using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCharStat : MonoBehaviour
{
    #region Поля
    //Переменная, содержащая в себе номер текущей главы.
    [SerializeField] private int chapterNumber;
    //Переменная, содержащая в себе количество убитых врагов.
    [SerializeField] private int enemyKilled;
    //Переменная, содержащая в себе количество заработанных очков.
    [SerializeField] private int pointsEarned;
    //Переменная, содержащая в себе время прохождения текущей главы.
    [SerializeField] private float chapterTime;
    #endregion

    #region Свойства
    //Свойства для получения значений полей.
    public int ChapterNumber { get { return chapterNumber; } }
    public int EnemyKilled { get { return enemyKilled; } }
    public int PointsEarned { get { return pointsEarned; } }
    public float ChapterTime { get { return chapterTime; } }
    #endregion

    #region Методы
    /* В методе OnValidate проверяем, не изменены ли
     * указанные параметры. Если они не равны необходимым значениям,
     * им присваивается ноль, в случае в номером главы - номер сцены.
     */
    private void OnValidate()
    {
        if (chapterNumber != SceneManager.GetActiveScene().buildIndex)
        {
            chapterNumber = SceneManager.GetActiveScene().buildIndex;
        }
        else if (enemyKilled != 0)
        {
            enemyKilled = 0;
        }
        else if (pointsEarned != 0)
        {
            pointsEarned = 0;
        }
        else if (chapterTime != 0)
        {
            chapterTime = 0;
        }
    }

    //Получаем текущий номер сцены и обнуляем статистику.
    private void Start()
    {
        chapterNumber = SceneManager.GetActiveScene().buildIndex;
        enemyKilled = 0;
        pointsEarned = 0;
        chapterTime = 0;
    }

    //Накручивает таймер уровня.
    private void Update()
    {
        chapterTime += Time.deltaTime;
    }

    /// <summary>
    /// Обновляет счет при смерти врага.
    /// </summary>
    /// <param name="enemyPointCost">Стоимость убийства врага в очках.</param>
    public void UpdateScoreOnEnemyDeath(int enemyPointCost)
    {
        pointsEarned += enemyPointCost;
        enemyKilled++;
    }
    #endregion
}
