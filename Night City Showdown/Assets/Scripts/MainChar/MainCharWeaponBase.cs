using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharWeaponBase : MonoBehaviour
{
    #region Поля
    //Статическое поле, содержащее наименование активного оружия.
    private static string activeWeaponType;
    //Статическое поле, содержащее лист доступных у игрока оружий.
    private static List<string> playerWeaponsList = new List<string>();
    //Переменная, обозначающая экипировано ли какое-либо оружие дальнего боя.
    protected bool isWeaponEquipped => activeWeaponType == "Pistol" || activeWeaponType == "Rifle";
    //Статичное поле, обозначающее, может ли атаковать игрок.
    protected static bool canAttack = true;
    //Спрайт рендерер игрока.
    protected SpriteRenderer playerSR;
    //Переменная, хранящая референс на класс с инпутом игрока.
    protected Project.Inputs.MainCharInput playerInput;
    //Переменная, хранящая референс на класс со звуками игрока.
    protected MainCharSounds playerSounds;
    //Переменная, хранящая референс на класс со здоровьем игрока.
    protected Health playerHealth;
    #endregion

    #region Свойства
    //Свойства для доступа к полям.
    public string ActiveWeaponType { get { return activeWeaponType; } }
    public List<string> PlayerWeaponsList { get { return playerWeaponsList; } }
    public bool IsWeaponEquipped { get { return isWeaponEquipped; } }
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    #endregion

    #region Методы
    //Метод, получающий компоненты.
    protected void GetNecessaryComponents()
    {
        playerSR = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<Project.Inputs.MainCharInput>();
        playerHealth = GetComponent<Health>();
        playerSounds = GetComponent<MainCharSounds>();
    }

    /// <summary>
    /// Метод добавляет в список оружий игрока новое наименование.
    /// </summary>
    /// <param name="AddedWeapon">Строковое наименование добавляемого оружия.</param>
    public void AddWeaponToList(string AddedWeapon)
    {
        playerWeaponsList.Add(AddedWeapon);
    }

    /// <summary>
    /// Метод меняет наименование активного оружия игрока.
    /// </summary>
    /// <param name="ActiveWeapon">Строковое наименование активного оружия игрока.</param>
    public void ChangeActiveWeapon(string ActiveWeapon)
    {
        activeWeaponType = ActiveWeapon;
    }
    #endregion
}
