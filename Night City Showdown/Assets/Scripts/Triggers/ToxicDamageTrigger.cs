using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicDamageTrigger : MonoBehaviour
{
    #region Переменные
    [Header("Timer every iteration of which damage take to player")]
    [SerializeField] private float damageTimer;

    [Header("Damage which take to player")]
    [SerializeField] private float damage;

    [Header("Player's GameObject")]
    [SerializeField] private GameObject player;

    //Таймер нанесения урона для обнуления.
    private float currentDamageTimer;

    //Переменная bool для отслеживания в триггере игрок или нет.
    private bool isInTrigger;

    //Переменная bool для отслеживания нанесен ли игроку урон в эту итерацию.
    private bool isDamaged;
    #endregion

    #region Методы
    /// <summary>
    /// На старте передаем в таймер для обнуления
    /// таймер нанесения урона.
    /// </summary>
    private void Awake()
    {
        currentDamageTimer = damageTimer;
    }

    /// <summary>
    /// В Update, если игрок в триггере, запускаем таймер нанесения урона.
    /// Когда он истекает, наносим урон оди раз. Обнуляем таймер.
    /// </summary>
    private void Update()
    {
        if (isInTrigger)
        {
            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0)
            {
                isDamaged = false;

                if (isDamaged == false)
                {
                    player.GetComponent<Health>().ToDamage(damage);
                    damageTimer = currentDamageTimer;
                    isDamaged = true;
                }
            }
        }
    }

    /// <summary>
    /// При вхождении в триггер переключается состояние,
    /// в триггере ли игрок.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInTrigger = true;
        }
    }

    /// <summary>
    /// При выходе из триггера, обновляется состояние, что игрок больше не в триггере.
    /// Обнуляем таймер нанесения урона.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInTrigger = false;
            damageTimer = currentDamageTimer;
        }
    }

    #endregion
}
