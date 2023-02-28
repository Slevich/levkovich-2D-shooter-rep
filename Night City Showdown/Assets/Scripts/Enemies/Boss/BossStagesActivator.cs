using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossStagesActivator : MonoBehaviour
{
    #region Поля
    [Header("Timeline, which plays, when enemy HP level falls down under 50%.")]
    [SerializeField] private PlayableDirector nextStageTimeline;
    [SerializeField] private float nextStageHealthProcent;

    //Компонент здоровья врага.
    private EnemyHealth enemyHealth;
    //Десятичное число с текущим процентом здоровья игрока
    private float healthProcent;
    #endregion

    #region Методы
    //Получаем компонент здоровья врага.
    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    /*Расчитываем процент здоровья игрока.
     *Если процент здоровья упал ниже установленного,
     *проигрываем таймлайн со следующей стадией.
     */
    private void Update()
    {
        CalculateHealthProcent();

        if (CheckHealthProcent())
        {
            nextStageTimeline.Play();
        }
    }

    private void CalculateHealthProcent()
    {
        healthProcent = enemyHealth.GetCurrentHealthProcent();
    }

    private bool CheckHealthProcent() => healthProcent <= nextStageHealthProcent;
    #endregion
}
