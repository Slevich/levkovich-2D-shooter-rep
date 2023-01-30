using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyObject : MonoBehaviour
{
    #region Методы
    /// <summary>
    /// Метод самоуничтожает объект,
    /// на котором висит скрипт.
    /// </summary>
    public void DestroySelfObject()
    {
        Destroy(gameObject);
    }
    #endregion
}
