using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelScreenActivatorTrigger : MonoBehaviour
{
    #region Переменные
    //Переменная, содержащая референс на компонент
    //с активацией экрана конца уровня.
    private EndLevelScreenActivator endLevelActivator;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компонент.
    /// </summary>
    private void Start()
    {
        endLevelActivator = GetComponent<EndLevelScreenActivator>();
    }

    /// <summary>
    /// При вхождении игрока в триггер,
    /// запускается метод с активацией
    /// экрана конца уровня.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            endLevelActivator.ActivateEndLevelScreen();
        }
    }
    #endregion
}
