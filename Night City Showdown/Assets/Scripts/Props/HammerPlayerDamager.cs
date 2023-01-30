using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPlayerDamager : MonoBehaviour
{
    #region Поля
    [Header("Damage, which apply to player's health.")]
    [SerializeField] private float damage;

    //Переменная, обозначающая попадает ли игрок в триггер.
    private bool isPlayerInRange;
    //Переменная с компонентом здоровья игрока.
    private Health playersHealth;
    #endregion

    #region Методы
    /* При попадании в триггер игрока, получаем компонент здоровья.
     * Переключаем переменную bool. В обратном случае наоборот.
     */ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playersHealth = collision.GetComponent<Health>();
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playersHealth = null;
            isPlayerInRange = false;
        }
    }

    /// <summary>
    /// Если игрок в триггере, наносим ему урон.
    /// </summary>
    public void DamagePlayer()
    {
        if (isPlayerInRange)
        {
            playersHealth.ToDamage(damage);
        }
    }
    #endregion
}
