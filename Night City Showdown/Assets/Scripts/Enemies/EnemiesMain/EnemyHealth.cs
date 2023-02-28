using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    #region Поля
    [Header("Game object prefab with explosion VFX which spawns on enemy death.")]
    [SerializeField] private GameObject deathVFXPrefab;
    [Header("Game object prefab with item which spawns on enemy death.")]
    [SerializeField] private GameObject spawnedOnDeathObjectPrefab;
    [Header("Game object prefab with explosion VFX which spawns on enemy death.")]
    [SerializeField] private float spawnPointDifference;
    [Header("Switch - is enemy spawn item on death.")]
    [SerializeField] private bool IsSpawnObjectOnDeath;
    [Header("Switch - is enemy destroy on death.")]
    [SerializeField] private bool IsDestroying = true;

    //Компоненты врага.
    private EnemyAnimation enemyAnim;
    private EnemyPointsManager enemyPointManager;
    private EnemyUIUpdater enemyUIUpdater;
    #endregion

    #region Методы
    //Получаем компоненты на старте.
    private void Awake()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyPointManager = GetComponent<EnemyPointsManager>();
        enemyUIUpdater = GetComponent<EnemyUIUpdater>();
    }

    /// <summary>
    /// Метод отнимает у текущего здоровья врага значение в размере урона.
    /// После чего обновляем Health Bar игрока.
    /// Если после нанесения урона, если враг жив, то
    /// Меняем анимационный стейт на смерть.
    /// Если враг спавнит объект при смерти, то делает это.
    /// Если враг уничтожается при смерти, спавнит эффект смерти,
    /// обновляем счет игрока и уничтожаем врага.
    /// </summary>
    /// <param name="damage">Amount of damage.</param>
    public override void ToDamage(float damage)
    {
        currentHealth -= damage;
        enemyUIUpdater.UpdateUIHealthBar(currentHealth, maxHealth);

        if (IsAlive == false)
        {
            enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Death);

            if (IsSpawnObjectOnDeath)
            {
                GameObject spawnedObject = Instantiate(spawnedOnDeathObjectPrefab,
                                                       new Vector2(transform.position.x, transform.position.y + spawnPointDifference),
                                                       Quaternion.identity);
            }

            if (IsDestroying)
            {
                GameObject deathVFX = Instantiate(deathVFXPrefab,
                                             new Vector2(transform.position.x, transform.position.y + spawnPointDifference),
                                             Quaternion.identity);
                enemyPointManager.ChangePlayerScore();
                Destroy(gameObject);
            }
        }
    }
    #endregion
}
