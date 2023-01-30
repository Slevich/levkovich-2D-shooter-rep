using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeTrigger : MonoBehaviour
{
    /// <summary>
    /// При вхождении в триггер в компоненте родительского объекта меняется состояние,
    /// согласно которому, в триггер врага попал коллайдер игрока.
    /// Также меняется состояние, что враг не двигается.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<EnemyAttack>().PlayerInRange = true;
            GetComponentInParent<EnemyMovement>().IsMoving = false;
        }
    }

    /// <summary>
    /// При выходе из триггера в компоненте родительского объекта меняется состояние,
    /// согласно которому, из триггера врага вышел коллайдер игрока.
    /// Также меняется состояние, что враг снова двигается.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<EnemyAttack>().PlayerInRange = false;
            GetComponentInParent<EnemyMovement>().IsMoving = true;
        }
    }
}
