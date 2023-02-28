using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    #region Поля
    [Header("Damage in melee to player health.")]
    [SerializeField] private float meleeDamage;
    [Header("Time interval between enemy attacks.")]
    [SerializeField] private float attackInterval;

    //Переключатель, обозначающий атакует ли враг.
    private bool isAttack;
    //Переменная для переприсваивания интервала атаки.
    private float reloadAttackInterval;
    //Компонент с анимациями врага.
    private EnemyAnimation enemyAnim;
    #endregion

    #region Методы
    //На старте получаем компонент и присваиваем переменной для обнуления атаки интервала.
    private void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        reloadAttackInterval = attackInterval;
    }

    /*Если игрок в радиусе атаки, вызываем метод атаки.
     * Если нет, игрок не атакует, обнуляем интервал атаки.
     */
    private void Update()
    {
        if (PlayerInRange)
        {
            EnemyMeleeHit();
        }
        else
        {
            enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
            isAttack = false;
            attackInterval = reloadAttackInterval;
        }
    }

    /*Метод меняет анимационный стейт на передвижение.
     * Запускаем таймер атаки.
     * Если таймер вышел  и враг может атаковать, меняем анимационный
     * стейт на атаку, если не может атаковать, обнуляем таймер атаки.
     * Если таймер не вышел, метод может атаковать.
     */
    private void EnemyMeleeHit()
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
        attackInterval -= Time.deltaTime;

        if (attackInterval <= 0)
        {
            if (isAttack) EnemyChangeOnAttackState();
            else attackInterval = reloadAttackInterval;
        }
        else isAttack = true;
    }

    //Метод меняет текущий анимационный стейт на атаку.
    private void EnemyChangeOnAttackState()
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Attack);
    }

    /// <summary>
    /// Если игрок в радиусе атаки и объект игрока не равен нулю, наносим игроку урон.
    /// </summary>
    public void DamagePlayerInMelee()
    {
        if (PlayerInRange && PlayerGameObject != null)
        {
            PlayerGameObject.GetComponent<Health>().ToDamage(meleeDamage);
        }
    }

    /// <summary>
    /// Метод переключает, что враг не может атаковать.
    /// </summary>
    public void StopAttack()
    {
        isAttack = false;
    }
    #endregion
}
