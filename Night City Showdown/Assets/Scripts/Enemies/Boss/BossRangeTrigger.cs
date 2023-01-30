using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRangeTrigger : MonoBehaviour
{
    #region Методы
    /// <summary>
    /// При вхождении игрока в триггер,
    /// он оказывается в радиусе атаки,
    /// что фиксируется в переменную.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<Boss>().playerInRange = true;
        }
    }

    /// <summary>
    /// При выходе игрока из триггера,
    /// он покидает радиус атаки,
    /// что фиксируется в переменную.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<Boss>().playerInRange = false;
        }
    }
    #endregion
}
