using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationActivateTrigger : MonoBehaviour
{
    #region Переменные
    //Компонент аниматор объекта, на котором висит скрипт.
    private Animator objectAnim;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компонент аниматор.
    /// </summary>
    private void Start()
    {
        objectAnim = GetComponentInParent<Animator>();
    }

    /// <summary>
    /// При вхождении в триггер объекта с тегом,
    /// переключается параметр аниматора.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroyable"))
        {
            objectAnim.SetBool("InRange", true);
        }
    }
    #endregion
}
