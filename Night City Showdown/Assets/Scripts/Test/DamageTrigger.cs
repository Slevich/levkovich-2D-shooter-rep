using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    //Этот скрипт нужен был для теста нанесения урона.
    //В самой игре он не задействован.

    #region Поля
    [Header("Number of damage, which apply to player's health.")]
    [SerializeField] private float damage;
    #endregion

    #region Методы
    // При вхождении в коллайдер, игроку наносится урон.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().ToDamage(damage);
        }
    }
    #endregion
}
