using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionAttack : EnemyAttack
{
    [SerializeField] private GameObject explosionVFXPrefab;
    [SerializeField] private float explosionDamage;

    private void Update()
    {
        if (PlayerInRange)
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        GameObject spawnedPrefab = Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);
        PlayerGameObject.GetComponent<Health>().ToDamage(explosionDamage);
        Destroy(gameObject);
    }
}
