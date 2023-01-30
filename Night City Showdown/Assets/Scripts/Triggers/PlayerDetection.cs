using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    #region Методы
    /// <summary>
    /// На старте получаем имя родительского объекта.
    /// </summary>

    /// <summary>
    /// При вхождении игрока в триггер, в компоненте родительского объекта,
    /// в зависимости от его имени переключается состояние - замечен игрок.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<EnemyMovement>().PlayerDetected = true;
            GetComponentInParent<EnemyMovement>().IsMoving = true;
            GetComponentInParent<EnemyAttack>().PlayerGameObject = collision.gameObject;
        }
    }

    /// <summary>
    /// При выходе игрока из триггера, в компоненте родительского объекта,
    /// в зависимости от его имени переключается состояние - игрок вышел из зоны.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<EnemyMovement>().PlayerDetected = false;
            GetComponentInParent<EnemyMovement>().IsMoving = false;
            GetComponentInParent<EnemyAttack>().PlayerGameObject = null;
        }
    }
    #endregion
}
