using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    #region Поля
    [Header("Game Object which destroy.")]
    [SerializeField] private GameObject destroyableGameObject;
    #endregion

    #region Методы
    /// <summary>
    /// Метод уничтожает игровой объект.
    /// </summary>
    public void ToDestroyObject()
    {
        Destroy(destroyableGameObject);
    }
    #endregion
}
