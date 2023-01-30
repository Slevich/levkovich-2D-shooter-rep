using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    #region Переменные
    [Header("Damage, which apply to characters health, when they comming to trigger.")]
    [SerializeField] private float deathDamage;
    #endregion

    #region Методы
    /// <summary>
    /// При вхождении персонажа в тегом в триггер,
    /// его здоровью наносится урон.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().ToDamage(deathDamage);
        }
    }
    #endregion
}
