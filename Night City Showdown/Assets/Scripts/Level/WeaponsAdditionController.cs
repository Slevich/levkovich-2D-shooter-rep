using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAdditionController : MonoBehaviour
{
    #region Переменные
    [Header("Main char Game Object.")]
    [SerializeField] private MainCharRangeAttack playerAttack;
    [Header("Names of weapon, which add to main character weapons list.")]
    [SerializeField] private string[] weaponNames;
    #endregion

    #region Методы
    //На старте, добавляет наименования в список оружий игрока.
    private void Start()
    {
        for (int i = 0; i < weaponNames.Length; i++)
        {
            playerAttack.AddWeaponToList(weaponNames[i]);
        }
    }
    #endregion
}
