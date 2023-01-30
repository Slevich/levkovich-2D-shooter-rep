using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    #region Переменные
    [Header("Game object, which activate, when player comming to trigger.")]
    [SerializeField] private GameObject activatedGameObject;
    #endregion

    #region Методы
    /// <summary>
    /// При вхождении в триггер, активируется конкретный
    /// игровой объект.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activatedGameObject.SetActive(true);
        }
    }
    #endregion
}
