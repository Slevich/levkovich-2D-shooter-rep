using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    #region Поля
    [Header("Number of ammo, on which all bullets count increase.")]
    [SerializeField] private float ammoIncrease;
    [Header("String name of box type: 'Pistol' or 'Rifle'.")]
    [SerializeField] private string ammoBoxType;
    #endregion

    #region Методы
    /* При вхождении в триггер, в зависимости от стрингового типа объекта
     * к количеству патронов определенного оружия прибавляется значение ammoIncrease.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ammoBoxType == "Pistol") collision.GetComponent<MainCharRangeAttack>().allPistolBullets += ammoIncrease;
            else if (ammoBoxType == "Rifle") collision.GetComponent<MainCharRangeAttack>().allRifleBullets += ammoIncrease;
            collision.GetComponent<MainCharSounds>().PlayAmmoPickingUpSound();
            Destroy(gameObject);
        }
    }
    #endregion
}
