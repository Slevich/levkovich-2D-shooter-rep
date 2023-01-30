using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    #region Методы
    /// <summary>
    /// При вхождении объекта с тегом "Destroyable"
    /// он уничтожается.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroyable"))
        {
            Destroy(collision.gameObject);
        }
    }
    #endregion
}
