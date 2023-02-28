using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeTrigger : MonoBehaviour
{
    /* При вхождении в триггер в компоненте родительского объекта меняется состояние,
     * согласно которому, в триггер врага попал коллайдер игрока.
     * Также меняется состояние, что враг не двигается.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && transform.parent.gameObject.activeInHierarchy)
        {
            GetComponentInParent<EnemyAttack>().PlayerInRange = true;
            GetComponentInParent<EnemyMovement>().IsMoving = false;
        }
    }

    /* При выходе из триггера в компоненте родительского объекта меняется состояние,
     * согласно которому, из триггера врага вышел коллайдер игрока.
     * Также меняется состояние, что враг снова двигается.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && transform.parent.gameObject.activeInHierarchy)
        {
            GetComponentInParent<EnemyAttack>().PlayerInRange = false;
            GetComponentInParent<EnemyMovement>().IsMoving = true;
        }
    }
}
