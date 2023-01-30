using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallCheckTrigger : MonoBehaviour
{
    #region Методы
    /// <summary>
    /// При вхождении в триггер в компоненте родительского объекта меняется состояние,
    /// согласно которому, в триггер врага попал коллайдер земли.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GetComponentInParent<EnemyMovement>().WallDetected = true;
        }
    }

    /// <summary>
    /// При выходе из триггера в компоненте родительского объекта меняется состояние,
    /// согласно которому, из триггера врага вышел коллайдер земли.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GetComponentInParent<EnemyMovement>().WallDetected = false;
        }
    }
    #endregion
}
