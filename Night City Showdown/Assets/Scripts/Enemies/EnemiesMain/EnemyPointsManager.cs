using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointsManager : MonoBehaviour
{
    #region Поля
    [Header("Cost in points of killing enemy.")]
    [SerializeField] private int enemyPointCost;
    [Header("Player component with level statistic.")]
    [SerializeField] private MainCharStat playerStat;
    #endregion

    #region Методы
    /// <summary>
    /// Метод вызывает у компонента игрока метод
    /// по изменению счета игрока.
    /// </summary>
    public void ChangePlayerScore()
    {
        playerStat.UpdateScoreOnEnemyDeath(enemyPointCost);
    }
    #endregion
}
