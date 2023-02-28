using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharRangeWeapons : MainCharWeaponBase
{
    #region Поля
    [Header("Game objects, which containts weapons to different directions.")]
    [SerializeField] protected GameObject pistolForwardHand;
    [SerializeField] protected GameObject pistolBackwardHand;
    [SerializeField] protected GameObject rifleForwardHands;
    [SerializeField] protected GameObject rifleBackwardHands;
    [Header("Game object, which contains all different weapon hands.")]
    [SerializeField] protected GameObject weaponHands;
    [Header("Transforms of every muzzle on the weapons.")]
    [SerializeField] protected Transform pistolForwardMuzzleflashPosition;
    [SerializeField] protected Transform pistolBackwardMuzzleflashPosition;
    [SerializeField] protected Transform rifleForwardMuzzleflashPosition;
    [SerializeField] protected Transform rifleBackwardMuzzleflashPosition;
    [Header("Bullets prefabs, which spawned on shooting.")]
    [SerializeField] protected GameObject pistolBulletPrefab;
    [SerializeField] protected GameObject rifleBulletPrefab;
    [Header("Game objects, which contains shoot animation for weapons.")]
    [SerializeField] protected GameObject pistolShootVFXPrefab;
    [SerializeField] protected GameObject rifleShootVFXPrefab;

    //Руки с текущим активным оружием при движении вперед или назад.
    protected GameObject forwardWeaponHands => ActiveWeaponType == "Pistol" ? pistolForwardHand : rifleForwardHands;
    protected GameObject backwardWeaponHands => ActiveWeaponType == "Pistol" ? pistolBackwardHand : rifleBackwardHands;
    //Переменная, хранящая в себе пулю активного оружия.
    protected GameObject activeBullet => ActiveWeaponType == "Pistol" ? pistolBulletPrefab : rifleBulletPrefab;
    //Переменная, хранящая в себе объект активного эффекта стрельбы.
    protected GameObject activeShootVFX => ActiveWeaponType == "Pistol" ? pistolShootVFXPrefab : rifleShootVFXPrefab;
    //Трансформ активной точки, из который будет вылетать пуля.
    protected Transform activeMuzzleflashPosition;
    //Количество пуль в магазине и общее количество пуль активного оружия.
    protected float activeBulletsInClip;
    protected float activeAllBullets;
    #endregion

    #region Методы
    /* Метод нужный для того, чтобы передавать в переменные
     * с активным видом оружия, те или иные характеристика видов оружия.
     * В зависимости от типа оружия, в переменные передаются характеристики
     * пистолета или винтовки.
     */
    protected void UpdateWeaponsTypeFeatures(float PistolBulletsInClip, float AllPistolBullets, float RifleBulletsInClip, float AllRifleBullets)
    {
        if (ActiveWeaponType == "Pistol")
        {
            rifleForwardHands.transform.parent.gameObject.SetActive(false);
            pistolForwardHand.transform.parent.gameObject.SetActive(true);
            activeBulletsInClip = PistolBulletsInClip;
            activeAllBullets = AllPistolBullets;

            if (playerSR.flipX)
            {
                activeMuzzleflashPosition = pistolBackwardMuzzleflashPosition;
            }
            else
            {
                activeMuzzleflashPosition = pistolForwardMuzzleflashPosition;
            }
        }
        else if (ActiveWeaponType == "Rifle")
        {
            pistolForwardHand.transform.parent.gameObject.SetActive(false);
            rifleForwardHands.transform.parent.gameObject.SetActive(true);
            activeBulletsInClip = RifleBulletsInClip;
            activeAllBullets = AllRifleBullets;

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

    // Метод, который переключает руки, в зависимости от поворота спрайта игрока.
    protected void FlipWeaponHands()
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
    #endregion
}
