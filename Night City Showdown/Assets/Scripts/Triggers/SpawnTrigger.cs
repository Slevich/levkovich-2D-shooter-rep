using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    #region Переменные
    [Header("Timer, every iteration of which object spwaned.")]
    [SerializeField] private float spawnTimer;
    [Header("Object prefab, which spawned.")]
    [SerializeField] private GameObject spawnedObject;
    
    //Переменная для обнуления таймера спавна.
    private float currentSpawnTimer;
    #endregion

    #region Методы
    /// <summary>
    /// На старте присваиваем таймеру обнуления таймер спавна.
    /// </summary>
    private void Start()
    {
        currentSpawnTimer = spawnTimer;
    }

    /// <summary>
    /// В апдейте запускаем таймер, по истечении которого
    /// спавнится префаб объекта.
    /// </summary>
    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnObject();
            spawnTimer = currentSpawnTimer;
        }
    }

    /// <summary>
    /// Метод спавнит новый игровой объект внутри объекта, 
    /// на котором висит скрипт.
    /// </summary>
    private void SpawnObject()
    {
        GameObject newGameObject = Instantiate(spawnedObject, transform.position, Quaternion.identity);
    }
    #endregion
}
