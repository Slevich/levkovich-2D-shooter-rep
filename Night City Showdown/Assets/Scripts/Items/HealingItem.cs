using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    #region Поля
    [Header("Number of health points, on which players's health increase.")]
    [SerializeField] private float healAmount;
    #endregion

    #region Методы
    /* При вхождении в триггер, у компонента здоровья игрока
     * вызывается метод, прибавляющий здоровье к текущему.
     * Проигрывается звук подбора хилки.
     * Объект самоуничтожается.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().ToHeal(healAmount);
            collision.GetComponent<MainCharSounds>().PlaySyringePickingUpSounds();
            Destroy(gameObject);
        }
    }
    #endregion
}
