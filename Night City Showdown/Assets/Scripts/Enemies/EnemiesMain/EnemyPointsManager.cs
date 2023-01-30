using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointsManager : MonoBehaviour
{
    [SerializeField] private int enemyPointCost;

    private EnemyMovement enemyMovement;
    private GameObject playerObject;
    private MainCharUICounts playerCounts;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        playerObject = enemyMovement.PlayerTransform.gameObject;
        playerCounts = playerObject.GetComponent<MainCharUICounts>();
    }

    public void ChangePlayerScore()
    {
        playerCounts.UpdateScoreOnEnemyDeath(enemyPointCost);
    }
}
