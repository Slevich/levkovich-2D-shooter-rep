using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCartCollider : MonoBehaviour
{
    #region Переменные
    //Переменная, содержащая референс на компонент врага.
    private BossCart bossCartComp;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компонент.
    /// </summary>
    private void Start()
    {
        bossCartComp = GetComponentInParent<BossCart>();
    }

    /// <summary>
    /// При столкновении врага с игроком,
    /// враг останавливается, вызывается метод
    /// нанесения урона.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && bossCartComp.isMoving)
        {
            bossCartComp.Damage(collision.rigidbody);
            bossCartComp.isMoving = false;
        }
    }
    #endregion
}
