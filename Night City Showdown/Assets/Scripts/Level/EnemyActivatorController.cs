using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivatorController : MonoBehaviour
{
    #region Переменные
    [Header("Component with health of first stage boss.")]
    [SerializeField] private Health bossHealth;
    [Header("Component with health of second stage boss.")]
    [SerializeField] private Health bossOnCartHealth;
    [Header("Enemy waves objects, which activate during the battle.")]
    [SerializeField] private GameObject[] enemyWaves;

    //Компонент позволяет хранить компонент с текущим здоровьем босса,
    //в зависимости от его стадии.
    private Health currentBossHealth;
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
    /// <summary>
    /// На старте сразу расчитываем процент, при котором активируем
    /// первую волну врагов. Обнуляем текущий номер волны.
    /// Присваиваем к переменной со здоровьем компонент босса первой фазы.
    /// </summary>
    private void Start()
    {
        bossHealthActivateProcent = 1 - procentStep;
        waveNumber = 0;
        currentBossHealth = bossHealth;
    }

    /// <summary>
    /// В Update мы рассчитываем текущий процент здоровья босса.
    /// Обновляем текущий компонент здоровья (при смене фазы).
    /// И если у нас процент становится меньше необходимого для активации волны,
    /// то активируем волну врагов.
    /// </summary>
    private void Update()
    {
        UpdateCurrentHealthComponent();
        CalculateCurrentProcent();

        if ((currentBossHealthProcent < bossHealthActivateProcent) && waveNumber < enemyWaves.Length)
        {
            enemyWaves[waveNumber].SetActive(true);
            waveNumber++;
            bossHealthActivateProcent -= procentStep;
        }
    }

    /// <summary>
    /// Метод рассчитывает процент текущего здоровья босса от максимального.
    /// </summary>
    private void CalculateCurrentProcent()
    {
        currentBossHealthProcent = currentBossHealth.GetCurrentHealthProcent();
    }

    /// <summary>
    /// Метод передает в переменную компонент здоровья босса первой или второй стадии,
    /// в зависимости от текущего процента здоровья босса.
    /// </summary>
    private void UpdateCurrentHealthComponent()
    {
        if (currentBossHealthProcent > 0.5f)
        {
            currentBossHealth = bossHealth;
        }
        else
        {
            currentBossHealth = bossOnCartHealth;
        }
    }
    #endregion
}
