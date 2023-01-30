using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelScreenActivator : MonoBehaviour
{
    #region Переменные
    [Header("Player HUD which visible on the gameplay.")]
    [SerializeField] private GameObject levelHUD;
    [Header("Panel with chapter score, which shows on the end of level.")]
    [SerializeField] private GameObject endLevelScreen;
    [Header("Text objects on 'End level screen' with chapter statistic info.")]
    [SerializeField] private Text chapterNumberText;
    [SerializeField] private Text enemiesKilledText;
    [SerializeField] private Text pointsEarnedText;
    [SerializeField] private Text chapterTimeMinutesText;
    [SerializeField] private Text chapterTimeSecondsText;
    [Header("Main character component with UI- and Counts-control.")]
    [SerializeField] private MainCharUICounts mainCharCounts;

    /// <summary>
    /// Переменные нужные для расчета из общего времени
    /// прохождения всей главы в минуты и секунды.
    /// </summary>
    private int chapterMinutes;
    private int chapterSeconds;
    #endregion

    #region Методы
    /// <summary>
    /// В методе Start минуты и секунды обнуляются.
    /// </summary>
    private void Start()
    {
        chapterMinutes = 0;
        chapterSeconds = 0;
    }

    /// <summary>
    /// Метод скрывает HUD уровня и активирует экран конца уровня.
    /// Ставит игру на паузу. Далее, переменные из компонента mainCharCounts,
    /// хранящие значения, передаются в текстовые поля.
    /// </summary>
    public void ActivateEndLevelScreen()
    {
        Time.timeScale = 0;
        levelHUD.SetActive(false);
        endLevelScreen.SetActive(true);
        chapterNumberText.text = mainCharCounts.chapterNumber.ToString();
        enemiesKilledText.text = mainCharCounts.enemyKilled.ToString();
        pointsEarnedText.text = mainCharCounts.pointsEarned.ToString();
        CalculateMinutesAndSeconds();
        chapterTimeMinutesText.text = chapterMinutes.ToString();
        chapterTimeSecondsText.text = chapterSeconds.ToString();
    }

    /// <summary>
    /// Метод вычисляет общее количество секунд и минут,
    /// затраченных на прохождение уровня.
    /// </summary>
    private void CalculateMinutesAndSeconds()
    {
        chapterMinutes = Convert.ToInt32(Mathf.Floor(mainCharCounts.chapterTime / 60));
        chapterSeconds = Convert.ToInt32(Mathf.Floor(mainCharCounts.chapterTime - (chapterMinutes * 60)));
    }
    #endregion
}
