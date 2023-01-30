using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    #region Поля
    [Header("String name of player's weapon: 'Pistol' or 'Rifle'")]
    [SerializeField] private string weaponHandsType;
    #endregion

    #region Методы
    /* При вхождении в триггер объекта, переключается переменная - экипировано ли оружие.
     * Активными руками становится подобранное оружие. В список оружий добавляется подобранное оружие.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<MainCharWeapons>().isWeaponEquipped = true;
            collision.GetComponent<MainCharWeapons>().activeWeaponHandsType = weaponHandsType;
            collision.GetComponent<MainCharWeapons>().playerWeaponsList.Add(weaponHandsType);
            collision.GetComponent<MainCharSounds>().PlayWeaponPickUpSound(weaponHandsType);
            Destroy(gameObject);
        }
    }
    #endregion
}