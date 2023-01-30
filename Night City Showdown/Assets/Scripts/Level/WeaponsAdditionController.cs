using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsAdditionController : MonoBehaviour
{
    #region Переменные
    [Header("Main char Game Object.")]
    [SerializeField] private GameObject playerCharacter;
    [Header("Names of weapon, which add to main character weapons list.")]
    [SerializeField] private string[] weaponNames;
    #endregion

    #region Методы
    private void Start()
    {
        for (int i = 0; i < weaponNames.Length; i++)
        {
           playerCharacter.GetComponent<MainCharWeapons>().playerWeaponsList.Add($"{weaponNames[i]}");
        }
    }
    #endregion
}
