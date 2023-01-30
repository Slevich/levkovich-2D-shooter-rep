using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class TimelineTrigger : MonoBehaviour
{
    #region Переменные
    [SerializeField] private PlayableDirector triggerTimeline;
    [SerializeField] private GameObject[] disactivatedObjects;
    [SerializeField] private GameObject[] activatedObjects;
    #endregion

    #region Методы
    /// <summary>
    /// При вхождении в триггер вызываются методы
    /// активации и дезактивациия объектов.
    /// После чего проигрывается таймлайн.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateObjects();
            DisactivateObjects();
            triggerTimeline.Play();
        }
    }

    /// <summary>
    /// Метод в цикле, если в массиве есть значения,
    /// постепенно дезактивирует все элементы.
    /// </summary>
    private void DisactivateObjects()
    {
        if (disactivatedObjects.Length > 0)
        {
            for (int a = 0; a < disactivatedObjects.Length; a++)
            {
                disactivatedObjects[a].SetActive(false);
            }
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Метод в цикле, если в массиве есть значения,
    /// постепенно активирует все элементы.
    /// </summary>
    private void ActivateObjects()
    {
        if (activatedObjects.Length > 0)
        {
            for (int i = 0; i < activatedObjects.Length; i++)
            {
                activatedObjects[i].SetActive(true);
            }
        }
    }
    #endregion
}
