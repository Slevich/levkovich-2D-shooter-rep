using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineCotroller : MonoBehaviour
{
    #region Поля
    [SerializeField] private GameObject[] activatedObjects;
    [SerializeField] private GameObject[] disactivatedObjects;
    #endregion

    #region Методы
    /// <summary>
    /// Метод активирует каждый объект из массива,
    /// при условии, что в массиве есть хотя бы один объект.
    /// </summary>
    public void ActivateObjects()
    {
        if (activatedObjects.Length > 0)
        {
            for (int i = 0; i < activatedObjects.Length; i++)
            {
                activatedObjects[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Метод дезактивирует каждый объект из массива,
    /// при условии, что в массиве есть хотя бы один объект.
    /// </summary>
    public void DisactivateObjects()
    {
        if (disactivatedObjects.Length > 0)
        {
            for (int a = 0; a < disactivatedObjects.Length; a++)
            {
                disactivatedObjects[a].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Метод дезактивирует один конкретный объект,
    /// который передается в качестве параметра.
    /// </summary>
    /// <param name="disactivatedObject">Игровой объект, подлежащий дизактивации.</param>
    public void DisactivateSingleObject(GameObject disactivatedObject)
    {
        disactivatedObject.SetActive(false);
    }

    /// <summary>
    /// Метод активирует один конкретный объект,
    /// который передается в качестве параметра.
    /// </summary>
    /// <param name="activatedObject">Игровой объект, подлежащий активации.</param>
    public void ActivateSingleObject(GameObject activatedObject)
    {
        activatedObject.SetActive(true);
    }
    #endregion
}
