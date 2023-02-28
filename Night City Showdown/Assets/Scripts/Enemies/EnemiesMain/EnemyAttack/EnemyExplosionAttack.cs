using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionAttack : EnemyAttack
{
    #region Поля
    [Header("Game object prefab with explosion VFX.")]
    [SerializeField] private GameObject explosionVFXPrefab;
    [Header("Damage to player health.")]
    [SerializeField] private float explosionDamage;
    #endregion

    #region Методы
    //В апдейте, если игрок в радиусе атаки, вызываем метод взрыва.
    private void Update()
    {
        if (PlayerInRange)
        {
            Explosion();
        }
    }

    //Метод спавнит префаб с эффектом взрыва, наносит урон игроку и самоуничтожает врага.
    private void Explosion()
    {
        GameObject spawnedPrefab = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
        PlayerGameObject.GetComponent<Health>().ToDamage(explosionDamage);
        Destroy(gameObject);
    }
    #endregion
}
