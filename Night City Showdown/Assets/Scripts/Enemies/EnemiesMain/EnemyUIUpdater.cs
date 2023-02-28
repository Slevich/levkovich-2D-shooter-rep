using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIUpdater : MonoBehaviour
{
    #region Поля
    [Header("Image on enemy canvas with Health Bar.")]
    [SerializeField] private Image healthBarImage;
    #endregion

    #region Методы
    /// <summary>
    /// Метод меняет заполнения Health Bar'а врага
    /// по проценту его текущего здоровья.
    /// </summary>
    /// <param name="CurrentHealth">Текущее количество здоровья врага.</param>
    /// <param name="MaxHealth">Максимальное количество здоровья врага.</param>
    public void UpdateUIHealthBar(float CurrentHealth, float MaxHealth)
    {
        healthBarImage.fillAmount = CurrentHealth / MaxHealth;
    }
    #endregion
}
