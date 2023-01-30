using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeBurstAttack : EnemyAttack
{
    #region Fields
    [Header("The amount of time that passes between attacks.")]
    [SerializeField] private float attackInterval;
    [Header("The amount of time that passes between the release of a new bullet.")]
    [SerializeField] private float attackFireRate;
    [Header("The number of bullets fired in a single attack.")]
    [SerializeField] private int bulletsPerAttack;
    [Header("The speed of the bullet firing.")]
    [SerializeField] private float fireSpeed;
    [Header("The value at which the enemy's aim is raised, relative to the player's transform.")]
    [SerializeField] private float lookOffset;
    [Header("Game objects, which contains weapon hands.")]
    [SerializeField] private GameObject weaponHands;
    [Header("Game object with forward weapon hands.")]
    [SerializeField] private GameObject weaponForwardHands;
    [Header("Game object with backward weapon hands.")]
    [SerializeField] private GameObject weaponBackwardHands;
    [Header("Game object prefab of bullet.")]
    [SerializeField] private GameObject weaponBulletPrefab;
    [Header("Game object prefab, which contains animation with shoot VFX.")]
    [SerializeField] private GameObject shootVFXPrefab;
    [Header("Transform components of game objects with muzzles positions.")]
    [SerializeField] private Transform weaponForwardMuzzlePosition;
    [SerializeField] private Transform weaponBackwardMuzzlePosition;

    private GroundedEnemyMovement enemyMovement;
    //Transform, который хранит активную точку, откуда вылетают пули.
    private Transform activeMuzzle;
    //Таймер интервала атаки для обнуления.
    private float reloadAttackInterval;
    //Таймер темпа огня для обнуления.
    private float reloadFireRate;
    //Количество пуль в атаке для обнуления.
    private int reloadBulletsPerAttack;
    #endregion

    #region Methods
    #region DefaultMethods
    private void Awake()
    {
        enemyMovement = GetComponent<GroundedEnemyMovement>();
        reloadAttackInterval = attackInterval;
        reloadBulletsPerAttack = bulletsPerAttack;
        reloadFireRate = attackFireRate;
    }

    private void Update()
    {
        UpdateWeaponHandsAndMuzzle();

        if (PlayerInRange)
        {
            BurstAttack();
        }
        else if (enemyMovement.PlayerDetected)
        {
            Aiming(new Vector2(enemyMovement.PlayerTransform.position.x, enemyMovement.PlayerTransform.position.y + lookOffset));
        }
        else
        {
            Aiming(new Vector2(enemyMovement.TargetPoint.x, weaponHands.transform.position.y));
            attackInterval = reloadAttackInterval;
            bulletsPerAttack = reloadBulletsPerAttack;
            attackFireRate = reloadFireRate;
        }
    }
    #endregion

    #region CustomMethods
    private void UpdateWeaponHandsAndMuzzle()
    {
        if (enemyMovement.IsLookForward)
        {
            activeMuzzle = weaponForwardMuzzlePosition;
            weaponBackwardHands.SetActive(false);
            weaponForwardHands.SetActive(true);
        }
        else
        {
            activeMuzzle = weaponBackwardMuzzlePosition;
            weaponForwardHands.SetActive(false);
            weaponBackwardHands.SetActive(true);
        }
    }

    private Vector2 CalculateBulletDirection()
    {
        Vector2 targetPosition = new Vector2(enemyMovement.PlayerTransform.position.x, enemyMovement.PlayerTransform.position.y + lookOffset);
        Vector2 heading = targetPosition - new Vector2(activeMuzzle.position.x, activeMuzzle.position.y);
        float bulletDistance = heading.magnitude;
        Vector2 bulletDirection;
        return bulletDirection = heading / bulletDistance;
    }

    private void BurstAttack()
    {
        Aiming(new Vector2(enemyMovement.PlayerTransform.position.x, enemyMovement.PlayerTransform.position.y + lookOffset));

        attackInterval -= Time.deltaTime;

        if (attackInterval <= 0)
        {
            attackFireRate -= Time.deltaTime;

            if (attackFireRate <= 0 && bulletsPerAttack > 0)
            {
                GameObject shootVFX = Instantiate(shootVFXPrefab, activeMuzzle.position, activeMuzzle.rotation);
                GameObject currentBullet = Instantiate(weaponBulletPrefab, activeMuzzle.position, activeMuzzle.rotation);
                Rigidbody2D currentBulletRB = currentBullet.GetComponent<Rigidbody2D>();
                currentBulletRB.velocity = CalculateBulletDirection() * fireSpeed;
                bulletsPerAttack--;
                attackFireRate = reloadFireRate;
            }
            else if (bulletsPerAttack <= 0)
            {
                bulletsPerAttack = reloadBulletsPerAttack;
                attackInterval = reloadAttackInterval;
            }
        }
    }

    private void Aiming(Vector2 AimTarget)
    {
        if (weaponForwardHands.activeInHierarchy)
        {
            weaponForwardHands.transform.LookAt(AimTarget);
        }
        else
        {
            weaponBackwardHands.transform.LookAt(AimTarget);
        }
    }

    #endregion

    #endregion
}
