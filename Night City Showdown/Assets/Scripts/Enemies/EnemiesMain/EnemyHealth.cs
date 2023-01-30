using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private GameObject spawnedOnDeathObjectPrefab;
    [SerializeField] private float spawnPointDifference;
    [SerializeField] private bool IsSpawnObjectOnDeath;

    private EnemyAnimation enemyAnim;
    private EnemyPointsManager enemyPointManager;
    private EnemyUIUpdater enemyUIUpdater;

    private void Awake()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyPointManager = GetComponent<EnemyPointsManager>();
        enemyUIUpdater = GetComponent<EnemyUIUpdater>();
    }

    public override void ToDamage(float damage)
    {
        currentHealth -= damage;
        enemyUIUpdater.UpdateUIHealthBar(currentHealth, maxHealth);

        if (IsAlive == false)
        {
            enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Death);
            GameObject deathVFX = Instantiate(deathVFXPrefab, 
                                              new Vector2(transform.position.x, transform.position.y + spawnPointDifference), 
                                              Quaternion.identity);

            if (IsSpawnObjectOnDeath)
            {
                GameObject spawnedObject = Instantiate(spawnedOnDeathObjectPrefab,
                                                       new Vector2(transform.position.x, transform.position.y + spawnPointDifference),
                                                       Quaternion.identity);
            }
            enemyPointManager.ChangePlayerScore();
            Destroy(gameObject);
        }
    }
}
