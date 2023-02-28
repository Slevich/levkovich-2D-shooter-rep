using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    #region Поля
    [Header("String name of player's weapon: 'Pistol' or 'Rifle'")]
    [SerializeField] private string weaponType;
    #endregion

    #region Методы
    /* При вхождении в триггер объекта, активным типом оружия становится подобранное оружие. 
     * В список оружий добавляется подобранное оружие.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<MainCharRangeAttack>().ChangeActiveWeapon(weaponType);
            collision.GetComponent<MainCharRangeAttack>().AddWeaponToList(weaponType);
            collision.GetComponent<MainCharSounds>().PlayWeaponPickUpSound(weaponType);
            Destroy(gameObject);
        }
    }
    #endregion
}