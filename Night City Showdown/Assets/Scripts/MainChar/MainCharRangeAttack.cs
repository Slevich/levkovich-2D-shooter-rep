using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharRangeAttack : MainCharRangeWeapons
{
    #region Поля
    [Header("Amout of speed, according of which the bullet is flying.")]
    [SerializeField] private float pistolFireSpeed;
    [SerializeField] private float rifleFireSpeed;
    [Header("The time interval after which a new bullet is fired from a rifle.")]
    [SerializeField] private float rifleFireRate;
    [Header("Amount of pistol bullets in clip and all.")]
    public float pistolBulletsInClip;
    public float allPistolBullets;
    [Header("Amount of rifle bullets in clip and all.")]
    public float rifleBulletsInClip;
    public float allRifleBullets;
    [Header("The time it takes to reload a weapon.")]
    public float pistolReloadTimer;
    public float rifleReloadTimer;

    //Обнуление таймеров перезарядки.
    private float pistolCurrentReloadTimer;
    private float rifleCurrentReloadTimer;
    //Перезаряжается ли игрок.
    private bool isReloading;
    //Переменная для хранения позиции курсора мыши.
    private Vector2 worldMousePosition;
    //Переменная, нужная для обнуления темпа стрельбы винтовки.
    private float currentRifleFireRate;
    //Стоимость перезарядки для видов оружия.
    private float pistolReloadCost;
    private float rifleReloadCost;
    //Стоимость перезарядки для активного вида оружия..
    private float activeReloadCost => ActiveWeaponType == "Pistol" ? pistolReloadCost : rifleReloadCost;
    //Переменные для обнуления таймеров перезарядки активного оружия.
    private float activeCurrentReloadTimer => ActiveWeaponType == "Pistol" ? pistolCurrentReloadTimer : rifleCurrentReloadTimer;
    #endregion

    #region Свойства
    public bool IsReloading { get { return isReloading; } set { isReloading = value; } }
    public float PistolCurrentReloadTimer { get { return pistolCurrentReloadTimer; } }
    public float RifleCurrentReloadTimer { get { return rifleCurrentReloadTimer; } } 
    #endregion

    #region Методы
    /* На старте получаем компоненты.
     * Присваиваем таймеры в переменные для обнуления.
     * Получаем стоимость перезарядки для оружий.
     * Включаем объект с руками для оружий.
     */
    private void Start()
    {
        GetNecessaryComponents();
        currentRifleFireRate = rifleFireRate;
        pistolCurrentReloadTimer = pistolReloadTimer;
        rifleCurrentReloadTimer = rifleReloadTimer;
        pistolReloadCost = pistolBulletsInClip;
        rifleReloadCost = rifleBulletsInClip;
    }
    
    private void Update()
    {
        if (canAttack)
        {
            if (playerHealth.IsAlive)
            {
                if (isWeaponEquipped)
                {
                    weaponHands.SetActive(true);
                    UpdateWeaponsTypeFeatures(pistolBulletsInClip, allPistolBullets, rifleBulletsInClip, allRifleBullets);
                    FlipWeaponHands();
                    WeaponLookAtMouse();
                    SwitchWeapons();
                    ShootWeapon();
                    ReloadWeapon();
                }
                else
                {
                    SwitchWeapons();
                }
            }
            else
            {
                weaponHands.SetActive(false);
            }
        }
    }

    // Метод формирует положение мыши в пространстве.
    private void WeaponLookAtMouse()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (backwardWeaponHands.activeInHierarchy) backwardWeaponHands.transform.LookAt(worldMousePosition);
        else if (forwardWeaponHands.activeInHierarchy) forwardWeaponHands.transform.LookAt(worldMousePosition);
    }

    /* Метод, в зависимости от нажатой кнопки, переключается на то или иной оружие.
     * При этом, отжимается нажатая кнопка смены оружия.
     */
    private void SwitchWeapons()
    {
        if (playerInput.Number1Pressed && PlayerWeaponsList.Contains("Fists"))
        {
            ChangeActiveWeapon("Fists");
            playerInput.Number1Pressed = false;
            OnSwitchWeapon(false);
        }
        else if (playerInput.Number2Pressed && PlayerWeaponsList.Contains("Pistol"))
        {
            ChangeActiveWeapon("Pistol");
            playerInput.Number2Pressed = false;
            OnSwitchWeapon(true);
        }
        else if (playerInput.Number3Pressed && PlayerWeaponsList.Contains("Rifle"))
        {
            ChangeActiveWeapon("Rifle");
            playerInput.Number3Pressed = false;
            OnSwitchWeapon(true);
        }
    }

    //Метод переключает отображение рук с оружием и сбрасывает перезарядку.
    private void OnSwitchWeapon(bool IsWeaponHandsActive)
    {
        weaponHands.SetActive(IsWeaponHandsActive);
        pistolReloadTimer = pistolCurrentReloadTimer;
        rifleReloadTimer = rifleCurrentReloadTimer;
        isReloading = false;
    }

    /* Метод в зависимости от типа оружия, спавнит пулю,
     * расчитывая, а затем передавая в неё направление движения.
     * При этом количество пуль в магазине активного оружия уменьшается на один
     * за каждую выпущенную пулю. Также проигрывается звук выстрела.
     * Если пуль в магазине нет, стрельба невозможна, проигрывается звук пустого
     * спуска крючка.
     */
    private void ShootWeapon()
    {
        if (playerInput.FireButtonPressed && activeBulletsInClip > 0 && isReloading == false)
        {
            if (ActiveWeaponType == "Pistol")
            {
                OnBulletShoot(pistolFireSpeed);
                playerSounds.PlayPistolShootSound();
                playerInput.FireButtonPressed = false;
            }
            else if (ActiveWeaponType == "Rifle")
            {
                rifleFireRate -= Time.deltaTime;

                if (rifleFireRate <= 0)
                {
                    OnBulletShoot(rifleFireSpeed);
                    playerSounds.PlayRifleShootSound();
                    rifleFireRate = currentRifleFireRate;
                }
            }
        }
        else if (playerInput.FireButtonPressed && activeBulletsInClip == 0 && isReloading == false)
        {
            if (ActiveWeaponType == "Pistol")
            {
                playerSounds.PlayPistolEmptyClipSound();
                playerInput.FireButtonPressed = false;
            }
            else if (ActiveWeaponType == "Rifle")
            {
                playerSounds.PlayRifleEmptyClipSound();
                playerInput.FireButtonPressed = false;
            }
        }
    }

    /*При выстреле, спавним префаб с эффектом стрельбы и пулю.
     *После чего, передаем пуле направление движения. Отнимаем патрон из магазина,
     *и в UI обновляем счетчик патронов
     */
    private void OnBulletShoot(float WeaponFireSpeed)
    {
        GameObject ShootVFX = Instantiate(activeShootVFX, activeMuzzleflashPosition.position, activeMuzzleflashPosition.rotation);
        GameObject currentBullet = Instantiate(activeBullet, activeMuzzleflashPosition.position, activeMuzzleflashPosition.rotation);
        Rigidbody2D currentBulletRB = currentBullet.GetComponent<Rigidbody2D>();
        currentBulletRB.velocity = CalculateBulletDirection() * WeaponFireSpeed;
        activeBulletsInClip--;
        UpdateWeaponsAmmoCounts();
    }

    /* Метод перезаряжает оружие, рассчитывая количество патронов в магазине и всего.
     * После чего перерасчитываются счетчики патронов для активного оружия.
     */
    private void ReloadWeapon()
    {
        if (playerInput.ReloadButtonPressed && isReloading && activeBulletsInClip < activeAllBullets)
        {
            if (ActiveWeaponType == "Pistol")
            {
                pistolReloadTimer -= Time.deltaTime;
                playerSounds.PlayPistolReloadSound();

                if (pistolReloadTimer <= 0)
                {
                    activeAllBullets += activeBulletsInClip;
                    activeBulletsInClip = 0;

                    if ((activeAllBullets - activeReloadCost) <= 0)
                    {
                        activeBulletsInClip += activeAllBullets;
                        activeAllBullets -= activeAllBullets;
                    }
                    else
                    {
                        activeAllBullets -= activeReloadCost;
                        activeBulletsInClip = activeReloadCost;
                    }

                    UpdateWeaponsAmmoCounts();
                    pistolReloadTimer = activeCurrentReloadTimer;
                    playerInput.ReloadButtonPressed = false;
                    isReloading = false;
                }
            }
            else if (ActiveWeaponType == "Rifle")
            {
                rifleReloadTimer -= Time.deltaTime;
                playerSounds.PlayRifleReloadSound();

                if (rifleReloadTimer <= 0)
                {
                    activeAllBullets += activeBulletsInClip;
                    activeBulletsInClip = 0;

                    if ((activeAllBullets - activeReloadCost) <= 0)
                    {
                        activeBulletsInClip += activeAllBullets;
                        activeAllBullets -= activeAllBullets;
                    }
                    else
                    {
                        activeAllBullets -= activeReloadCost;
                        activeBulletsInClip = activeReloadCost;
                    }

                    UpdateWeaponsAmmoCounts();
                    rifleReloadTimer = activeCurrentReloadTimer;
                    playerInput.ReloadButtonPressed = false;
                    isReloading = false;
                }
            }
        }
        else
        {
            isReloading = false;
        }
    }

    // Метод рассчитывает направление полета пули, в соответствии с положением мыши на экране.
    private Vector3 CalculateBulletDirection()
    {
        Vector3 mousePositionPoint = new Vector3(worldMousePosition.x, worldMousePosition.y, 0);
        Vector3 heading = mousePositionPoint - activeMuzzleflashPosition.position;
        float bulletDistance = heading.magnitude;
        Vector3 bulletDirection = heading / bulletDistance;
        return bulletDirection;
    }

    /* Метод подгружает в переменные с пулями в магазине и всеми пулями оружий
     * те значения, которые хранятся в переменных активного оружия.
     */
    private void UpdateWeaponsAmmoCounts()
    {
        if (ActiveWeaponType == "Pistol")
        {
            allPistolBullets = activeAllBullets;
            pistolBulletsInClip = activeBulletsInClip;
        }
        else if (ActiveWeaponType == "Rifle")
        {
            allRifleBullets = activeAllBullets;
            rifleBulletsInClip = activeBulletsInClip;
        }
    }
    #endregion
}
