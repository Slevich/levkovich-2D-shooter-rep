using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : Health
{
    #region Поля
    [Header("Game object prefab, which spawn on object death.")]
    [SerializeField] private GameObject deathVFXPrefab;
    [Header("Amount of unity on X coordinates, from which prefab spawns.")]
    [SerializeField] private float spawnPointDifference;
    //Компонент с UI.
    private EnemyUIUpdater enemyUIUpdater;
    #endregion

    #region Методы
    //Получаем компонент.
    private void Awake()
    {
        enemyUIUpdater = GetComponent<EnemyUIUpdater>();
    }
    /*Метод отнимает тикущее здоровье и обновляет Health Bar объекта.
     *Если объект уничтожен, спавнит эффект смерти и самоуничтожаем его.
     */
    public override void ToDamage(float damage)
    {
        currentHealth -= damage;
        enemyUIUpdater.UpdateUIHealthBar(currentHealth, maxHealth);

        if (IsAlive == false)
        {
            GameObject deathVFX = Instantiate(deathVFXPrefab,
                                             new Vector2(transform.position.x, transform.position.y + spawnPointDifference),
                                             Quaternion.identity);
            Destroy(gameObject);
        }
    }
    #endregion
}
