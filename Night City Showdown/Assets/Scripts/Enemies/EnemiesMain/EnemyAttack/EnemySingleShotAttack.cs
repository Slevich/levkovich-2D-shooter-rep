using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySingleShotAttack : EnemyAttack
{
    [Header("The amount of time that passes between attacks.")]
    [SerializeField] private float attackInterval;
    [Header("The speed of the bullet firing.")]
    [SerializeField] private float fireSpeed;
    [Header("The difference in distance along the X-axis within which the enemy has no change in animation position.")]
    [SerializeField] private float distanceDifference;
    [Header("Game object prefab of bullet.")]
    [SerializeField] private GameObject weaponBulletPrefab;
    [Header("Game object prefab, which contains animation with shoot VFX.")]
    [SerializeField] private GameObject shootVFXPrefab;
    [Header("Transform array with all muzzles.")]
    [SerializeField] private Transform[] muzzles;

    private GroundedRangeSingleShotEnemyAnimations enemyAnim;
    private SpriteRenderer enemySR;
    private Transform playerTransform;
    //Массив трансформов активных точек вылета пуль.
    private Transform[] activeMuzzles = new Transform[2];
    //Точка, обозначающая направление полета обеих пуль.
    private Vector3 bullet1Direction;
    private Vector3 bullet2Direction;
    //Таймер интервала атаки для обнуления.
    private float reloadAttackInterval;
    //Направление атаки врага.
    private float attackDirection;

    public float AttackDirection { get { return attackDirection; } }

    private void Awake()
    {
        playerTransform = GetComponent<GroundedEnemyMovement>().PlayerTransform;
        enemySR = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<GroundedRangeSingleShotEnemyAnimations>();
        reloadAttackInterval = attackInterval;
        attackDirection = 0;
    }

    private void Update()
    {
        if (PlayerInRange)
        {
            UpdateActiveMuzzles();
            SingleShotAttack();
        }
        else
        {
            attackInterval = reloadAttackInterval;
        }
    }

    /// <summary>
    /// Метод передает в направление полета пули -
    /// весторы right, чтобы из точек они летели прямо.
    /// </summary>
    private void CalculateBulletDirections()
    {
        bullet1Direction = activeMuzzles[0].right;
        bullet2Direction = activeMuzzles[1].right;
    }

    /// <summary>
    /// Метод передает в качестве активных точек вылета пули
    /// местоположения объектов, в зависимости от местоположения игрока.
    /// Если он намного выше врага, берутся верхние точки.
    /// Если на одной уровне - средние точки.
    /// Если сильно ниже - нижние точки.
    /// </summary>
    private void UpdateActiveMuzzles()
    {
        if (enemySR.flipX)
        {
            if ((playerTransform.position.y < (transform.position.y + distanceDifference)) && (playerTransform.position.y > (transform.position.y - distanceDifference)))
            {
                activeMuzzles[0] = muzzles[8];
                activeMuzzles[1] = muzzles[9];
                attackDirection = 0;
            }
            else if (playerTransform.position.y < (transform.position.y - distanceDifference))
            {
                activeMuzzles[0] = muzzles[6];
                activeMuzzles[1] = muzzles[7];
                attackDirection = -1;
            }
            else if (playerTransform.position.y > (transform.position.y + distanceDifference))
            {
                activeMuzzles[0] = muzzles[10];
                activeMuzzles[1] = muzzles[11];
                attackDirection = 1;
            }
        }
        else
        {
            if ((playerTransform.position.y < (transform.position.y + distanceDifference)) && (playerTransform.position.y > (transform.position.y - distanceDifference)))
            {
                activeMuzzles[0] = muzzles[2];
                activeMuzzles[1] = muzzles[3];
                attackDirection = 0;
            }
            else if (playerTransform.position.y < (transform.position.y - distanceDifference))
            {
                activeMuzzles[0] = muzzles[0];
                activeMuzzles[1] = muzzles[1];
                attackDirection = -1;
            }
            else if (playerTransform.position.y > (transform.position.y + distanceDifference))
            {
                activeMuzzles[0] = muzzles[4];
                activeMuzzles[1] = muzzles[5];
                attackDirection = 1;
            }
        }
    }

    /// <summary>
    /// Метод переключает состояние врага в покой.
    /// Запускает таймер, по результатам которого стейт
    /// переключается в атаку.
    /// </summary>
    private void SingleShotAttack()
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
        attackInterval -= Time.deltaTime;

        if (attackInterval <= 0)
        {
            enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Attack);
        }
    }

    /// <summary>
    /// Метод спавнит две пули,
    /// и они летят в направлении игрока.
    /// Метод вызывается в анимации стельбы через ивент.
    /// </summary>
    private void ShootBullets()
    {
        GameObject shootVFX1 = Instantiate(shootVFXPrefab, activeMuzzles[0].position, activeMuzzles[0].rotation);
        GameObject currentBullet1 = Instantiate(weaponBulletPrefab, activeMuzzles[0].position, activeMuzzles[0].rotation);
        Rigidbody2D currentBullet1RB = currentBullet1.GetComponent<Rigidbody2D>();
        GameObject shootVFX2 = Instantiate(shootVFXPrefab, activeMuzzles[1].position, activeMuzzles[1].rotation);
        GameObject currentBullet2 = Instantiate(weaponBulletPrefab, activeMuzzles[1].position, activeMuzzles[1].rotation);
        Rigidbody2D currentBullet2RB = currentBullet2.GetComponent<Rigidbody2D>();
        CalculateBulletDirections();
        currentBullet1RB.velocity = bullet1Direction * fireSpeed;
        currentBullet2RB.velocity = bullet2Direction * fireSpeed;
    }

    /// <summary>
    /// Метод обнуляет интервал атаки.
    /// Метод вызывается в конце анимации стрельбы
    /// через ивент.
    /// </summary>
    private void ReloadAttackInterval()
    {
        attackInterval = reloadAttackInterval;
    }
}
