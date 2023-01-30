using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharWeapons : MonoBehaviour
{
    #region Переменные
    [Header("Game objects, which containts weapons to different directions.")]
    [SerializeField] private GameObject pistolForwardHand;
    [SerializeField] private GameObject pistolBackwardHand;
    [SerializeField] private GameObject rifleForwardHands;
    [SerializeField] private GameObject rifleBackwardHands;
    [Header("Game object, which contains all different weapon hands.")]
    [SerializeField] private GameObject weaponHands;
    [Header("Transforms of every muzzle on the weapons.")]
    [SerializeField] private Transform pistolForwardMuzzleflashPosition;
    [SerializeField] private Transform pistolBackwardMuzzleflashPosition;
    [SerializeField] private Transform rifleForwardMuzzleflashPosition;
    [SerializeField] private Transform rifleBackwardMuzzleflashPosition;
    [Header("Bullets prefabs, which spawned on shooting.")]
    [SerializeField] private GameObject pistolBulletPrefab;
    [SerializeField] private GameObject rifleBulletPrefab;
    [Header("Game objects, which contains shoot animation for weapons.")]
    [SerializeField] private GameObject pistolShootVFXPrefab;
    [SerializeField] private GameObject rifleShootVFXPrefab;
    [Header("Amout of speed, according of which the bullet is flying.")]
    [SerializeField] private float pistolFireSpeed;
    [SerializeField] private float rifleFireSpeed;
    [Header("The time interval after which a new bullet is fired from a rifle.")]
    [SerializeField] private float rifleFireRate;
    [Header("Damage amount, which applies to enemies health, when character hit by fist.")]
    [SerializeField] private float hitDamage;
    [Header("Damage amount, which applies to enemies health, when character punch by foot")]
    [SerializeField] private float punchDamage;
    [Header("Amount of force, which applies to enemy Rigidbody, to push him, when character punch by foot")]
    [SerializeField] private float pushForce;
    
    [Header("Amount of pistol bullets in clip and all.")]
    public float pistolBulletsInClip;
    public float allPistolBullets;
    [Header("Amount of rifle bullets in clip and all.")]
    public float rifleBulletsInClip;
    public float allRifleBullets;
    [Header("The time it takes to reload a weapon.")]
    public float pistolReloadTimer;
    public float rifleReloadTimer;
    [Header("String name of active weapon.")]
    public string activeWeaponHandsType;
    [Header("List of the names of the weapons available to the player.")]
    public List<string> playerWeaponsList = new List<string>();
    
    //Коллайдер врага.
    [HideInInspector]
    public Collider2D enemyCollider;

    //Переменные для обнуления таймеров перезарядки оружий.
    [HideInInspector]
    public float pistolCurrentReloadTimer;
    [HideInInspector]
    public float rifleCurrentReloadTimer;

    //Далее идут переменные, обозначающие разные состояния.
    //Они скрыты, чтобы не перегружать инспектор.
    [HideInInspector]
    public bool isWeaponEquipped;
    [HideInInspector]
    public bool isReloading;
    [HideInInspector]
    public bool isHitting;
    [HideInInspector]
    public bool isPunching;
    [HideInInspector]
    public bool enemyInRange;
    [HideInInspector]
    public bool canAim;

    //Переменная для хранения позиции курсора мыши.
    private Vector2 worldMousePosition;
    //Переменная для хранения направления полета пули.
    private Vector3 bulletDirection;
    //Игровой объект, который хранит в себе трансформ
    //активной точки, из который будет вылетать пуля.
    private Transform activeMuzzleflashPosition;
    //Спрайт рендерер игрока.
    private SpriteRenderer playerSR;
    //Игровые объекты, которые хранят в себе руки с
    //текущим активным оружием.
    private GameObject forwardWeaponHands;
    private GameObject backwardWeaponHands;
    //Переменная, хранящая референс на класс с инпутом игрока.
    private Project.Inputs.MainCharInput playerInput;
    //Переменная, хранящая референс на класс со звуками игрока.
    private MainCharSounds playerSounds;
    //Переменная, хранящая референс на класс со здоровьем игрока.
    private Health playerHealth;
    //Переменная, хранящая в себе пулю активного оружия.
    private GameObject activeBullet;
    //Переменная, хранящая в себе объект активного эффекта стрельбы.
    private GameObject activeShootVFX;
    //Переменная, нужная для обнуления темпа стрельбы винтовки.
    private float currentRifleFireRate;
    //Переменные, хранящие количество пуль в магазине и общее количество пуль
    //активного оружия.
    private float activeBulletsInClip;
    private float activeAllBullets;
    //Переменные, хранящие в себе стоимость перезарядки для видов оружия.
    private float pistolReloadCost;
    private float rifleReloadCost;
    //Переменная, хранящая в себе стоимость перезарядки для активного вида оружия..
    private float activeReloadCost;
    //Переменные для обнуления таймеров перезарядки активного оружия.
    private float activeCurrentReloadTimer;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компоненты.
    /// После чего добавляем кулаки в доступное оружие и делаем их оружием по умолчанию.
    /// Присваиваем таймеры в переменные для обнуления.
    /// Получаем стоимость перезарядки для оружий.
    /// Переводим bool переменные в нужные положения.
    /// Включаем объект с руками для оружий.
    /// </summary>
    private void Start()
    {
        playerSR = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<Project.Inputs.MainCharInput>();
        playerHealth = GetComponent<Health>();
        playerSounds = GetComponent<MainCharSounds>();
        playerWeaponsList.Add("Fists");
        activeWeaponHandsType = "Fists";
        currentRifleFireRate = rifleFireRate;
        pistolCurrentReloadTimer = pistolReloadTimer;
        rifleCurrentReloadTimer = rifleReloadTimer;
        pistolReloadCost = pistolBulletsInClip;
        rifleReloadCost = rifleBulletsInClip;
        canAim = true;
        enemyCollider = null;
        isPunching = false;
        isHitting = false;
        weaponHands.SetActive(true);
    }
    
    /// <summary>
    /// В Update, если в руках пистолет или винтовка - оружие экипировано.
    /// Если есть разрешение на оружие, и игрок жив, выполняем методы.
    /// Если игрок умер, отключаем руки с оружием.
    /// </summary>
    private void Update()
    {
        if (activeWeaponHandsType == "Pistol" || activeWeaponHandsType == "Rifle")
        {
            isWeaponEquipped = true;
        }
        else
        {
            isWeaponEquipped = false;
        }

        if (canAim)
        {
            if (playerHealth.IsAlive)
            {
                if (isWeaponEquipped)
                {
                    weaponHands.SetActive(true);
                    UpdateWeaponsTypeFeatures();
                    FlipWeaponHands();
                    WeaponLookAtMouse();
                    SwitchWeapons();
                    MuzzleFlashRaycast();
                    ShootWeapon();
                    ReloadWeapon();
                }
                else
                {
                    UpdateWeaponsTypeFeatures();
                    SwitchWeapons();
                    HitSomeone();
                }
            }
            else
            {
                weaponHands.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Метод нужный для того, чтобы передавать в переменные
    /// с активным видом оружия, те или иные характеристика видов оружия.
    /// В зависимости от типа оружия, в переменные передаются характеристики
    /// пистолета или винтовки.
    /// </summary>
    private void UpdateWeaponsTypeFeatures()
    {
        if (activeWeaponHandsType == "Pistol")
        {
            rifleForwardHands.SetActive(false);
            rifleBackwardHands.SetActive(false);
            forwardWeaponHands = pistolForwardHand;
            backwardWeaponHands = pistolBackwardHand;
            activeBullet = pistolBulletPrefab;
            activeShootVFX = pistolShootVFXPrefab;
            activeBulletsInClip = pistolBulletsInClip;
            activeAllBullets = allPistolBullets;
            activeReloadCost = pistolReloadCost;
            activeCurrentReloadTimer = pistolCurrentReloadTimer;

            if (playerSR.flipX)
            {
                activeMuzzleflashPosition = pistolBackwardMuzzleflashPosition;
            }
            else
            {
                activeMuzzleflashPosition = pistolForwardMuzzleflashPosition;
            }
        }
        else if (activeWeaponHandsType == "Rifle")
        {
            pistolForwardHand.SetActive(false);
            pistolBackwardHand.SetActive(false);
            forwardWeaponHands = rifleForwardHands;
            backwardWeaponHands = rifleBackwardHands;
            activeBullet = rifleBulletPrefab;
            activeShootVFX = rifleShootVFXPrefab;
            activeBulletsInClip = rifleBulletsInClip;
            activeAllBullets = allRifleBullets;
            activeReloadCost = rifleReloadCost;
            activeCurrentReloadTimer = rifleCurrentReloadTimer;

            if (playerSR.flipX)
            {
                activeMuzzleflashPosition = rifleBackwardMuzzleflashPosition;
            }
            else
            {
                activeMuzzleflashPosition = rifleForwardMuzzleflashPosition;
            }
        }
    }

    /// <summary>
    /// Метод кидает луч, показывающий направление полета пули из активной точки (ствола).
    /// </summary>
    private void MuzzleFlashRaycast()
    {
        Physics2D.Raycast(activeMuzzleflashPosition.position, activeMuzzleflashPosition.TransformDirection(Vector2.right), 10);
        Debug.DrawRay(activeMuzzleflashPosition.position, activeMuzzleflashPosition.TransformDirection(Vector2.right), Color.green);
    }

    /// <summary>
    /// Метод, который переключает руки, в зависимости от поворота спрайта игрока.
    /// </summary>
    private void FlipWeaponHands()
    {
        if (playerSR.flipX)
        {
            forwardWeaponHands.SetActive(false);
            backwardWeaponHands.SetActive(true);
        }
        else
        {
            backwardWeaponHands.SetActive(false);
            forwardWeaponHands.SetActive(true);
        }
    }

    /// <summary>
    /// Метод формирует положение мыши в пространстве.
    /// Активные руки, если они активны, смотря в сторону мыши.
    /// </summary>
    private void WeaponLookAtMouse()
    {
        worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (backwardWeaponHands.activeInHierarchy)
        {
            backwardWeaponHands.transform.LookAt(worldMousePosition);
        }
        else if (forwardWeaponHands.activeInHierarchy)
        {
            forwardWeaponHands.transform.LookAt(worldMousePosition);
        }
    }

    /// <summary>
    /// Метод, в зависимости от нажатой кнопки, переключается на то или иной оружие.
    /// При этом меняется активное оружие в руках.
    /// Обнуляются таймеры перезарядки.
    /// </summary>
    private void SwitchWeapons()
    {
        if (playerInput.Number1Pressed && playerWeaponsList.Contains("Fists"))
        {
            activeWeaponHandsType = "Fists";
            weaponHands.SetActive(false);
            playerInput.Number1Pressed = false;
            pistolReloadTimer = pistolCurrentReloadTimer;
            rifleReloadTimer = rifleCurrentReloadTimer;
            isReloading = false;
        }
        else if (playerInput.Number2Pressed && playerWeaponsList.Contains("Pistol"))
        {
            activeWeaponHandsType = "Pistol";
            weaponHands.SetActive(true);
            playerInput.Number2Pressed = false;
            pistolReloadTimer = pistolCurrentReloadTimer;
            rifleReloadTimer = rifleCurrentReloadTimer;
            isReloading = false;
        }
        else if (playerInput.Number3Pressed && playerWeaponsList.Contains("Rifle"))
        {
            activeWeaponHandsType = "Rifle";
            weaponHands.SetActive(true);
            playerInput.Number3Pressed = false;
            pistolReloadTimer = pistolCurrentReloadTimer;
            rifleReloadTimer = rifleCurrentReloadTimer;
            isReloading = false;
        }
    }

    /// <summary>
    /// Метод в зависимости от типа оружия, спавнит пулю,
    /// расчитывая, а затем передавая в неё направление движения.
    /// При этом количество пуль в магазине активного оружия уменьшается на один
    /// за каждую выпущенную пулю.
    /// Также проигрывается зву выстрела.
    /// Если пуль в магазине нет, стрельба невозможна, проигрывается звук пустого
    /// спуска крючка.
    /// </summary>
    private void ShootWeapon()
    {
        if (playerInput.FireButtonPressed && activeBulletsInClip > 0 && isReloading == false)
        {
            if (activeWeaponHandsType == "Pistol")
            {
                GameObject pistolShootVFX = Instantiate(activeShootVFX, activeMuzzleflashPosition.position, activeMuzzleflashPosition.rotation);
                GameObject currentBullet = Instantiate(activeBullet, activeMuzzleflashPosition.position, activeMuzzleflashPosition.rotation);
                Rigidbody2D currentBulletRB = currentBullet.GetComponent<Rigidbody2D>();
                CalculateBulletDirection();
                currentBulletRB.velocity = bulletDirection * pistolFireSpeed;
                activeBulletsInClip--;
                UpdateWeaponsAmmoCounts();
                playerSounds.PlayPistolShootSound();
                playerInput.FireButtonPressed = false;
            }
            else if (activeWeaponHandsType == "Rifle")
            {
                rifleFireRate -= Time.deltaTime;

                if (rifleFireRate <= 0)
                {
                    GameObject rifleShootVFX = Instantiate(activeShootVFX, activeMuzzleflashPosition.position, activeMuzzleflashPosition.rotation);
                    GameObject currentBullet = Instantiate(activeBullet, activeMuzzleflashPosition.position, activeMuzzleflashPosition.rotation);
                    Rigidbody2D currentBulletRB = currentBullet.GetComponent<Rigidbody2D>();
                    CalculateBulletDirection();
                    currentBulletRB.velocity = bulletDirection * rifleFireSpeed;
                    activeBulletsInClip--;
                    UpdateWeaponsAmmoCounts();
                    playerSounds.PlayRifleShootSound();
                    rifleFireRate = currentRifleFireRate;
                }
            }
        }
        else if (playerInput.FireButtonPressed && activeBulletsInClip == 0 && isReloading == false)
        {
            if (activeWeaponHandsType == "Pistol")
            {
                playerSounds.PlayPistolEmptyClipSound();
                playerInput.FireButtonPressed = false;
            }
            else if (activeWeaponHandsType == "Rifle")
            {
                playerSounds.PlayRifleEmptyClipSound();
                playerInput.FireButtonPressed = false;
            }
        }
    }

    /// <summary>
    /// Метод перезаряжает оружие, рассчитывая количество патронов в магазине и всего.
    /// После чего перерасчитываются счетчики патронов для активного оружия.
    /// </summary>
    private void ReloadWeapon()
    {
        if (playerInput.ReloadButtonPressed && isReloading && activeAllBullets > 0)
        {
            if (activeWeaponHandsType == "Pistol")
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
            else if (activeWeaponHandsType == "Rifle")
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

    /// <summary>
    /// Метод рассчитывает направление полета пули, в соответствии с положением мыши на экране.
    /// </summary>
    private void CalculateBulletDirection()
    {
        Vector3 mousePositionPoint = new Vector3(worldMousePosition.x, worldMousePosition.y, 0);
        Vector3 heading = mousePositionPoint - activeMuzzleflashPosition.position;
        float bulletDistance = heading.magnitude;
        bulletDirection = heading / bulletDistance;
    }

    /// <summary>
    /// Метод, переключает переменные, если нажата клавиша, что игрок бьет рукой или пинаетю
    /// </summary>
    private void HitSomeone()
    {
        if (activeWeaponHandsType == "Fists" && playerInput.FireButtonPressed && isPunching == false)
        {
            isHitting = true;
        }
        else if (activeWeaponHandsType == "Fists" && playerInput.PunchButtonPressed && isHitting == false)
        {
            isPunching = true;
        }
    }

    /// <summary>
    /// Метод позволяет прекратить проигрывание анимации (она проигрывается один раз),
    /// даже если кнопка удара или пинка продолжает быть нажатой.
    /// </summary>
    public void UnHit()
    {
        isHitting = false;
        playerInput.FireButtonPressed = false;
        isPunching = false;
        playerInput.PunchButtonPressed = false;
    }

    /// <summary>
    /// Метод наносит урон противнику, находящемуся в зоне атаки ближнего боя,
    /// а также толкает его в нужном направлении (только при пинке).
    /// </summary>
    public void DamageInMelee()
    {
        if (isHitting && enemyInRange && enemyCollider != null)
        {
            enemyCollider.GetComponent<Health>().ToDamage(hitDamage);
            playerSounds.PlayHitSound();
        }
        else if (isPunching && enemyInRange && enemyCollider != null)
        {
            enemyCollider.GetComponent<Health>().ToDamage(punchDamage);
            playerSounds.PlayHitSound();

            if (playerSR.flipX)
            {
                enemyCollider.attachedRigidbody.AddForce(Vector2.left * pushForce, ForceMode2D.Impulse);
            }
            else
            {
                enemyCollider.attachedRigidbody.AddForce(Vector2.right * pushForce, ForceMode2D.Impulse);
            }
        }
    }

    /// <summary>
    /// Метод подгружает в переменные с пулями в магазине и всеми пулями оружий
    /// те значения, которые хранятся в переменных активного оружия.
    /// </summary>
    private void UpdateWeaponsAmmoCounts()
    {
        if (activeWeaponHandsType == "Pistol")
        {
            allPistolBullets = activeAllBullets;
            pistolBulletsInClip = activeBulletsInClip;
        }
        else if (activeWeaponHandsType == "Rifle")
        {
            allRifleBullets = activeAllBullets;
            rifleBulletsInClip = activeBulletsInClip;
        }
    }
    #endregion
}
