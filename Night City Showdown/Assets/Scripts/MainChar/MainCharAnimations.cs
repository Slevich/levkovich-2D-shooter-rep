using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharAnimations : MonoBehaviour
{
    #region Переменные
    //Различные необходимые классы.
    private Animator playerAnim;
    private SpriteRenderer playerSR;
    private MainCharMovement playerMovement;
    private MainCharWeapons playerWeapons;
    private Project.Inputs.MainCharInput playerInput;
    private Health playerHealth;
    #endregion

    #region Методы
    /* На старте даем разрешение анимировать персонажа.
     * Получаем соответствующие компоненты.
     */
    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerSR = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<MainCharMovement>();
        playerWeapons = GetComponent<MainCharWeapons>();
        playerInput = GetComponent<Project.Inputs.MainCharInput>();
        playerHealth = GetComponent<Health>();
    }

    /* В Update, если разрешено анимировать,
     * вызываем методы по обновлению анимаций персонажа.
     */
    private void Update()
    {
        UpdateMovementAnimations();
        UpdateJumpAnimation();
        UpdateDoubleJumpAnimation();
        UpdateWeaponAnimations();
        UpdateDeathAnimation();
        UpdateHitAnimation();
        UpdatePunchAnimation();
    }

    /* В навизимости от горизонтального направления инпута,
     * передаем эти параметры в аниматор.
     * Также поворачиваем спрайт, в зависимости от направления.
     */
    private void UpdateMovementAnimations()
    {
        if (playerInput.HorizontalDirection == 0)
        {
            playerAnim.SetFloat("Speed", playerInput.HorizontalDirection);
        }
        else if (playerInput.HorizontalDirection > 0 && playerHealth.IsAlive)
        {
            playerSR.flipX = false;
            playerAnim.SetFloat("Speed", playerInput.HorizontalDirection);
        }
        else if (playerInput.HorizontalDirection < 0 && playerHealth.IsAlive)
        {
            playerSR.flipX = true;
            playerAnim.SetFloat("Speed", Mathf.Abs(playerInput.HorizontalDirection));
        }
    }

    /* Если получили состояние из компонента передвижения,
     * что игрок прыгает, передаем соответствующие данные
     * в аниматор.
     */
    private void UpdateJumpAnimation()
    {
        if (playerMovement.isJumping)
        {
            playerAnim.SetBool("IsJumping", true);
            playerAnim.SetFloat("JumpPosition", playerMovement.jumpPosition);
        }
        else
        {
            playerAnim.SetFloat("JumpPosition", playerMovement.jumpPosition);
            playerAnim.SetBool("IsJumping", false);
        }
    }

    /* Если получили состояние из компонента передвижения,
     *  что игрок совершает двойной прыжок, передаем соответствующие данные
     *  в аниматор.
     */
    private void UpdateDoubleJumpAnimation()
    {
        if (playerMovement.isDoubleJumping)
        {
            playerAnim.SetBool("IsDoubleJumping", true);
            playerAnim.SetFloat("DoubleJumpPosition", playerMovement.doubleJumpPosition);
        }
        else
        {
            playerAnim.SetFloat("DoubleJumpPosition", playerMovement.doubleJumpPosition);
            playerAnim.SetBool("IsDoubleJumping", false);
        }
    }

    /* Если оружие экипировано (не кулаки), то в зависимости
     * от типа оружия в руках, переключаем параметры аниматора.
     * Если оружие не экипировано, аниматор в безоружном состояниии
     * (активное оружие - кулаки).
     */
    private void UpdateWeaponAnimations()
    {
        if (playerWeapons.isWeaponEquipped)
        {
            if (playerWeapons.activeWeaponHandsType == "Pistol")
            {
                playerAnim.SetBool("IsPistolEquipped", true);
                playerAnim.SetBool("IsRifleEquipped", false);
            }
            else if (playerWeapons.activeWeaponHandsType == "Rifle")
            {
                playerAnim.SetBool("IsRifleEquipped", true);
                playerAnim.SetBool("IsPistolEquipped", false);
            }
        }
        else
        {
            if (playerWeapons.activeWeaponHandsType == "Pistol")
            {
                playerAnim.SetBool("IsPistolEquipped", false);
            }
            else if (playerWeapons.activeWeaponHandsType == "Rifle")
            {
                playerAnim.SetBool("IsRifleEquipped", false);
            }
            else if (playerWeapons.activeWeaponHandsType == "Fists")
            {
                playerAnim.SetBool("IsPistolEquipped", false);
                playerAnim.SetBool("IsRifleEquipped", false);
            }
        }
    }


    // Если персонаж бьет кулаком, изменяем в аниматоре состояние.
    private void UpdateHitAnimation()
    {
        if (playerWeapons.isHitting)
        {
            playerAnim.SetBool("IsHitting", true);
        }
        else
        {
            playerAnim.SetBool("IsHitting", false);
        }
    }

    // Если игрок бьет ногой, изменяем в аниматоре состояние.
    private void UpdatePunchAnimation()
    {
        if (playerWeapons.isPunching)
        {
            playerAnim.SetBool("IsPunching", true);
        }
        else
        {
            playerAnim.SetBool("IsPunching", false);
        }
    }

    /* Если получаем, что игрок мертв, то передаем в параметр аниматоре, 
     * что нужно проиграть анимацию смерти.
     */
    private void UpdateDeathAnimation()
    {
        if (playerHealth.IsAlive == false)
        {
            playerAnim.SetBool("IsDead", true);
        }
    }
    #endregion
}
