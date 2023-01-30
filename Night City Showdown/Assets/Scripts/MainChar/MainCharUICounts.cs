using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainCharUICounts : MonoBehaviour
{
    #region Поля
    [Header("Text components of objects, which represents weapon bullets counts.")]
    [SerializeField] private Text pistolAllBulletsCount;
    [SerializeField] private Text pistolClipBulletsCount;
    [SerializeField] private Text rifleAllBulletsCount;
    [SerializeField] private Text rifleClipBulletsCount;
    [Header("Text component of objects, which represents current score amount.")]
    [SerializeField] private Text scoreCountText;
    [Header("Game objects with image filters, which overlaid on weapons HUD panels.")]
    [SerializeField] private GameObject pistolDisableFilter;
    [SerializeField] private GameObject rifleDisableFilter;
    [SerializeField] private GameObject fistsDisableFilter;
    [Header("Game objects (panels) with weapons information.")]
    [SerializeField] private GameObject pistolPanel;
    [SerializeField] private GameObject riflePanel;
    [Header("Game objects which represents indicator of ready to shoot.")]
    [SerializeField] private GameObject pistolActiveShootingPanel;
    [SerializeField] private GameObject pistolUnactiveShootingPanel;
    [SerializeField] private GameObject rifleActiveShootingPanel;
    [SerializeField] private GameObject rifleUnactiveShootingPanel;
    [Header("Images (filters) which overlaid on weapons panel, when it reloading")]
    [SerializeField] private Image pistolReloadPanel;
    [SerializeField] private Image rifleReloadPanel;
    [Header("Image, which reflect current level of character's HP.")]
    [SerializeField] private Image playerHealthProgressBar;
    [Header("Game object which contains all HUD, which active on gameplay.")]
    [SerializeField] private GameObject levelHUD;
    [Header("Game object which contains screen on players death.")]
    [SerializeField] private GameObject deathScreen;
    [Header("Audio source with music, which played on the level.")]
    [SerializeField] private AudioSource levelMusicSource;

    //Переменная, содержащая в себе номер текущей главы.
    public int chapterNumber;
    //Переменная, содержащая в себе количество убитых врагов.
    public int enemyKilled;
    //Переменная, содержащая в себе количество заработанных очков.
    public int pointsEarned;
    //Переменная, содержащая в себе время прохождения текущей главы.
    public float chapterTime;

    //Переменные, содержащие в себе референс на другие компоненты персонажа.
    private MainCharWeapons playerWeapons;
    private MainCharSounds playerSounds;
    private Health playerHealth;
    #endregion

    #region Методы
    /* В методе OnValidate проверяем, не изменены ли
     * указанные параметры. Если они не равны необходимым значениям,
     * им присваивается ноль, в случае в номером главы - номер сцены.
     */
    private void OnValidate()
    {
        if (chapterNumber != SceneManager.GetActiveScene().buildIndex)
        {
            chapterNumber = SceneManager.GetActiveScene().buildIndex;
        }
        else if (enemyKilled != 0)
        {
            enemyKilled = 0;
        }
        else if (pointsEarned != 0)
        {
            pointsEarned = 0;
        }
        else if (chapterTime != 0)
        {
            chapterTime = 0;
        }
    }
    
    /* На старте получаем необходимые компоненты.
     * Запускаем метод, чтобы подтянуть значения переменных с патронами оружий
     * в поля с текстом в UI.
     * Присваиваем переменным нужные значения.
     */
    private void Start()
    {
        playerWeapons = GetComponent<MainCharWeapons>();
        playerSounds = GetComponent<MainCharSounds>();
        playerHealth = GetComponent<Health>();
        UpdateAmmoCounts();
        chapterNumber = SceneManager.GetActiveScene().buildIndex;
        enemyKilled = 0;
        pointsEarned = 0;
        chapterTime = 0;
    }

    /* В Update запускаем таймер прохождения главы.
     * После чего вызываем все необходимые методы.
     */
    private void Update()
    {
        chapterTime += Time.deltaTime;
        UpdateScoreCountText();
        UpdateAmmoCounts();
        UpdateWeaponDisableFilters();
        UpdateHealthLevel();
        ShowWeaponSlots();
        UpdateWeaponShootingCondition();
        UpdateReloadUIAnimation();
        ShowDeathScreen();
    }

    /* Метод подтягивает значения из компонента, управляющего
     * оружием персонажа. После чего передает их в текстовые поля в самом UI.
     */
    private void UpdateAmmoCounts()
    {
        pistolAllBulletsCount.text = playerWeapons.allPistolBullets.ToString();
        pistolClipBulletsCount.text = playerWeapons.pistolBulletsInClip.ToString();
        rifleAllBulletsCount.text = playerWeapons.allRifleBullets.ToString();
        rifleClipBulletsCount.text = playerWeapons.rifleBulletsInClip.ToString();
    }

    /* Метод переключает фильтры, накладываемые на панели с тем
     * или иным видом оружия, показывая какое сейчас активно.
     * Происходит это в зависимости от вида оружия в руках (или его отсутствия).
     */
    private void UpdateWeaponDisableFilters()
    {
        if (playerWeapons.activeWeaponHandsType == "Pistol")
        {
            pistolDisableFilter.SetActive(false);
            fistsDisableFilter.SetActive(true);
            rifleDisableFilter.SetActive(true);
        }
        else if (playerWeapons.activeWeaponHandsType == "Rifle")
        {
            pistolDisableFilter.SetActive(true);
            rifleDisableFilter.SetActive(false);
            fistsDisableFilter.SetActive(true);
        }
        else if (playerWeapons.activeWeaponHandsType == "Fists")
        {
            pistolDisableFilter.SetActive(true);
            rifleDisableFilter.SetActive(true);
            fistsDisableFilter.SetActive(false);
        }
    }

    /* Метод показывает на экране в UI панель с определенным видом оружия,
     * если оно содержится в листа имеющихся у персонажа.
     */ 
    private void ShowWeaponSlots()
    {
        if (playerWeapons.playerWeaponsList.Contains("Pistol")) pistolPanel.SetActive(true);
        else pistolPanel.SetActive(false);

        if (playerWeapons.playerWeaponsList.Contains("Rifle")) riflePanel.SetActive(true);
        else riflePanel.SetActive(false);
    }

    /* Метод расситывает сколько процентов составляет текущее здоровье
     * от максимального и передает в заполнение HP бара в UI.
     */ 
    private void UpdateHealthLevel()
    {
        playerHealthProgressBar.fillAmount = playerHealth.GetCurrentHealthProcent();
    }

    /* Метод проигрывает анимацию того, как при перезарядке
     * на панель оружия наслаивается фильтр, который уменьшается
     * в процессе перезарядки.
     */
    private void UpdateReloadUIAnimation()
    {
        if (playerWeapons.isReloading && playerWeapons.activeWeaponHandsType == "Pistol")
        {
            pistolReloadPanel.gameObject.SetActive(true);
            pistolReloadPanel.fillAmount = playerWeapons.pistolReloadTimer / playerWeapons.pistolCurrentReloadTimer;
        }
        else if (playerWeapons.isReloading && playerWeapons.activeWeaponHandsType == "Rifle")
        {
            rifleReloadPanel.gameObject.SetActive(true);
            rifleReloadPanel.fillAmount = playerWeapons.rifleReloadTimer / playerWeapons.rifleCurrentReloadTimer;
        }
        else
        {
            pistolReloadPanel.gameObject.SetActive(false);
            rifleReloadPanel.gameObject.SetActive(false);
        }
    }

    /* Метод активирует красный индикатор, если
     * в магазине оружия закончились патроны.
     * В обратном случае - индикатор зеленый.
     */
    private void UpdateWeaponShootingCondition()
    {
        if (playerWeapons.pistolBulletsInClip == 0)
        {
            pistolUnactiveShootingPanel.SetActive(true);
            pistolActiveShootingPanel.SetActive(false);
        }
        else
        {
            pistolUnactiveShootingPanel.SetActive(false);
            pistolActiveShootingPanel.SetActive(true);
        }

        if (playerWeapons.rifleBulletsInClip == 0)
        {
            rifleUnactiveShootingPanel.SetActive(true);
            rifleActiveShootingPanel.SetActive(false);
        }
        else
        {
            rifleUnactiveShootingPanel.SetActive(false);
            rifleActiveShootingPanel.SetActive(true);
        }
    }

    /* В случае, если игрок мертв (закончилось ХП),
     * активируется экран смерти. Фоновая музыка ставится на паузу.
     */ 
    private void ShowDeathScreen()
    {
        if (playerHealth.IsAlive == false)
        {
            levelHUD.SetActive(false);
            deathScreen.SetActive(true);
            levelMusicSource.Pause();
            playerSounds.PlayDeathSound();
        }
    }

    // Метод обновляет количество очков в UI.
    private void UpdateScoreCountText()
    {
        scoreCountText.text = pointsEarned.ToString();
    }

    /// <summary>
    /// Обновляет счет при смерти врага.
    /// </summary>
    /// <param name="enemyPointCost">Стоимость убийства врага в очках.</param>
    public void UpdateScoreOnEnemyDeath(int enemyPointCost)
    {
        pointsEarned += enemyPointCost;
        enemyKilled++;
    }
    #endregion
}
