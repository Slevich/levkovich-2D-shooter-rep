using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    #region Переменные
    [Header("Number of ammo, on which all bullets count increase.")]
    [SerializeField] private float ammoIncrease;
    [Header("String name of box type: 'Pistol' or 'Rifle'.")]
    [SerializeField] private string ammoBoxType;
    #endregion

    #region Методы
    /// <summary>
    /// При вхождении в триггер, в зависимости от стрингового типа объекта
    /// к количеству патронов определенного оружия прибавляется значение ammoIncrease.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ammoBoxType == "Pistol") collision.GetComponent<MainCharWeapons>().allPistolBullets += ammoIncrease;
            else if (ammoBoxType == "Rifle") collision.GetComponent<MainCharWeapons>().allRifleBullets += ammoIncrease;
            collision.GetComponent<MainCharSounds>().PlayAmmoPickingUpSound();
            Destroy(gameObject);
        }
    }
    #endregion
}
