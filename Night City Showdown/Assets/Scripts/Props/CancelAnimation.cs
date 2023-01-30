using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelAnimation : MonoBehaviour
{
    #region Поля
    [Header("String name of bool variable in Animator")]
    [SerializeField] private string animBoolName;
    
    //Аниматор объекта.
    private Animator objectAnim;
    #endregion

    #region Методы
    // На старте получаем аниматор.
    private void Start()
    {
        objectAnim = GetComponent<Animator>();
    }

    /// <summary>
    /// Метод передает значение в аниматоре.
    /// </summary>
    public void ToCancelAnimation()
    {
        objectAnim.SetBool(animBoolName, false);
    }
    #endregion
}
