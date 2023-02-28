using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivatorController : MonoBehaviour
{
    #region Поля
    [Header("Component with health of first stage boss.")]
    [SerializeField] private EnemyHealth bossHealth;
    [Header("Component with health of second stage boss.")]
    [SerializeField] private EnemyHealth bossOnCartHealth;
    [Header("Enemy waves objects, which activate during the battle.")]
    [SerializeField] private GameObject[] enemyWaves;

    //Компонент позволяет хранить компонент с текущим здоровьем босса,
    //в зависимости от его стадии.
    private EnemyHealth currentBossHealth;
    //Текущий процент здоровья босса.
    private float currentBossHealthProcent;
    //Процент здоровья босса, на котором активируется группа врагов.
    private float bossHealthActivateProcent;
    //Шаг, на который уменьшается процент, в который активируется группа врагов.
    private float procentStep = 0.2f;
    //Некущий номер волны врагов.
    private int waveNumber;
    #endregion

    #region Методы
    /* На старте сразу расчитываем процент, при котором активируем
     * первую волну врагов. Обнуляем текущий номер волны.
     * Присваиваем к переменной со здоровьем компонент босса первой фазы.
     */
    private void Start()
    {
        bossHealthActivateProcent = 1 - procentStep;
        waveNumber = 0;
        currentBossHealth = bossHealth;
    }

    /* В Update мы рассчитываем текущий процент здоровья босса.
     * Обновляем текущий компонент здоровья (при смене фазы).
     * И если у нас процент становится меньше необходимого для активации волны,
     * то активируем волну врагов.
     */
    private void Update()
    {
        currentBossHealthProcent = currentBossHealth.GetCurrentHealthProcent();
        CheckCurrentHealthComponent();

        if ((currentBossHealthProcent <= bossHealthActivateProcent) && waveNumber < enemyWaves.Length)
        {
            enemyWaves[waveNumber].SetActive(true);
            waveNumber++;
            bossHealthActivateProcent -= procentStep;
        }
    }

    //Если здоровье босса упало меньше 50%, переприсваивает компонент второй стадии босса.
    private void CheckCurrentHealthComponent()
    {
        if (currentBossHealthProcent <= 0.5f)
        {
            currentBossHealth = bossOnCartHealth;
        }
    }
    #endregion
}
