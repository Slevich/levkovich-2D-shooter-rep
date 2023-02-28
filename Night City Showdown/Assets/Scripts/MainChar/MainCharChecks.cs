using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharChecks : MonoBehaviour
{
    #region Поля
    [Header("Layer mask - 'Ground'.")]
    [SerializeField] private LayerMask groundMask;
    [Header("Layer mask - 'Enemy'.")]
    [SerializeField] private LayerMask enemyMask;
    [Header("Childer capsule collider used to check ground and enemies.")]
    [SerializeField] private CapsuleCollider2D groundCheckCapsule;

    //Переменная, обозначающая в воздухе ли игрок.
    private bool isInAir;
    #endregion

    #region Свойства
    public bool IsInAir { get { return isInAir; } }
    #endregion

    #region Методы
    //В апдейте проверяем в воздухе ли игрок.
    private void Update()
    {
        CheckIsInAir();
    }

    /* Метод кастует капсуль, который равен триггеру на персонаже.
     * Если игрок находится на земле или на враге, то он не воздухе.
     * Если же нет, тогда он в воздухе.
     */
    private void CheckIsInAir()
    {
        if (Physics2D.CapsuleCast(groundCheckCapsule.transform.position,
                                  groundCheckCapsule.size, groundCheckCapsule.direction,
                                  groundCheckCapsule.transform.rotation.z, Vector2.zero, 0f, groundMask))
        {
            isInAir = false;
        }
        else if (Physics2D.CapsuleCast(groundCheckCapsule.transform.position,
                                       groundCheckCapsule.size, groundCheckCapsule.direction,
                                       groundCheckCapsule.transform.rotation.z, Vector2.zero, 0f, enemyMask))
        {
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }
    }
    #endregion
}
