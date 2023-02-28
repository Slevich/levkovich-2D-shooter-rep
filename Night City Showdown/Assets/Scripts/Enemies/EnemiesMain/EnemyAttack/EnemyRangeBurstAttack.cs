using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeBurstAttack : EnemyAttack
{
    #region Поля
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

    //Компоненты врага.
    private SpriteRenderer enemySR;
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

    #region Методы
    #region Классические методы
    //Получаем компоненты и присваиваем значения переменным для обнуления.
    private void Awake()
    {
        enemyMovement = GetComponent<GroundedEnemyMovement>();
        enemySR = GetComponent<SpriteRenderer>();
        reloadAttackInterval = attackInterval;
        reloadBulletsPerAttack = bulletsPerAttack;
        reloadFireRate = attackFireRate;
    }

    /*В апдейте обновляем текущие активные руки с оружие и точку вылета пули.
     *Если игрок в радиусе атаки, поварачиваем спрайт в направлении игрока, запускаем атаку.
     *Если игрок замечен, целимся в него.
     *В противном случае, целимся в направлении движения и обнуляем таймеры.
     */
    private void Update()
    {
        UpdateWeaponHandsAndMuzzle();

        if (PlayerInRange)
        {
            enemySR.flipX = enemyMovement.PlayerTransform.position.x >= transform.position.x ? false : true;
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

    #region Созданные методы
    /*Если враг смотрит вперед, активная точка вылета пули - в передних руках.
     * При этом, переключает активные руки врага.
     * В обратном случае - наоборот.
     */
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

    //Метод рассчитывает направление полета пули относительно положения игрока.
    private Vector2 CalculateBulletDirection()
    {
        Vector2 targetPosition = new Vector2(enemyMovement.PlayerTransform.position.x, enemyMovement.PlayerTransform.position.y + lookOffset);
        Vector2 heading = targetPosition - new Vector2(activeMuzzle.position.x, activeMuzzle.position.y);
        float bulletDistance = heading.magnitude;
        Vector2 bulletDirection;
        return bulletDirection = heading / bulletDistance;
    }

    /*Во время атаки, враг целится в игрока.
     * Запускаем таймер интервала атаки, когда они истекает,
     * запускаем таймер между выстрелами.
     * Если таймер между пулями вышел и количество пуль за атаку больше нуля,
     * Спавним визуальный эффект пули и саму пулю, направляем её,
     * отнимаем единицу пули за атак и обнуляем таймер между пулями.
     * Если количество пуль за атаку стало меньше или равно нулю,
     * то обнуляем таймеры и пули за атаку.
     */
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

    //Метод направляет активные руки врага на определенную цель.
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
