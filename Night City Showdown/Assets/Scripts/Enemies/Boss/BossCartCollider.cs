using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCartCollider : MonoBehaviour
{
    #region Поля
    //Переменная, содержащая референс на компонент врага.
    private RamAttack bossAttack;
    #endregion

    #region Методы
    // На старте получаем компонент.
    private void Start()
    {
        bossAttack = GetComponentInParent<RamAttack>();
    }

    /*При вхождении коллизии с тегом игрока наносит урон.
     *При вхождении коллизии с тегом игрока или границей преключаем
     *состояние о том, что вхождение в триггер произошло.
     */ 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (bossAttack.IsMoving) bossAttack.DamagePlayer(collision.gameObject);
            bossAttack.BorderCollided = true;
        }
        else if (collision.CompareTag("Border"))
        {
            bossAttack.BorderCollided = true;
        }
    }
    #endregion
}
