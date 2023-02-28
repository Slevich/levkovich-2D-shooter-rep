using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    #region Методы
    /* При вхождении игрока в триггер, в компоненте родительского объекта,
     * в зависимости от его имени переключается состояние - замечен игрок.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && transform.parent.gameObject.activeInHierarchy)
        {
            GetComponentInParent<EnemyMovement>().PlayerDetected = true;
            GetComponentInParent<EnemyMovement>().IsMoving = true;
            GetComponentInParent<EnemyMovement>().PlayerTransform = collision.transform;
            GetComponentInParent<EnemyAttack>().PlayerGameObject = collision.gameObject;
        }
    }

    /* При выходе игрока из триггера, в компоненте родительского объекта,
     * в зависимости от его имени переключается состояние - игрок вышел из зоны.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && transform.parent.gameObject.activeInHierarchy)
        {
            GetComponentInParent<EnemyMovement>().PlayerDetected = false;
            GetComponentInParent<EnemyMovement>().IsMoving = false;
            GetComponentInParent<EnemyMovement>().PlayerTransform = null;
            GetComponentInParent<EnemyAttack>().PlayerGameObject = null;
        }
    }
    #endregion
}
