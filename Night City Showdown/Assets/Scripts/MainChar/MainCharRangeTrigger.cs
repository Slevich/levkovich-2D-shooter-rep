using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharRangeTrigger : MonoBehaviour
{
    #region Методы
    /* При попадании в триггер врага, меняем соответствующее состояние,
     * что враг находится в зоне атаки ближнего боя игрока.
     * Присваиваем в переменную коллайдера врага - коллайдер из триггера.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GetComponentInParent<MainCharMeleeAttack>().EnemyInRange = true;
            GetComponentInParent<MainCharMeleeAttack>().EnemyCollider = collision;
        }
    }

    /* При выходе врага из триггера, меняем соответствующее состояние,
     * что враг не находится в зоне атаки ближнего боя игрока.
     * Присваиваем в переменную коллайдера врага null.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GetComponentInParent<MainCharMeleeAttack>().EnemyInRange = false;
            GetComponentInParent<MainCharMeleeAttack>().EnemyCollider = null;
        }
    }
    #endregion
}
