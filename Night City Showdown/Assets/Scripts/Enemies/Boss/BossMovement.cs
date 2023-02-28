using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    #region Поля
    //Поля с компонентами врага.
    private EnemyAttack enemyAttack;
    private EnemyAnimation enemyAnim;
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;
    #endregion

    #region Методы
    //На старте получаем компоненты и меняем анимационный стейт на движение.
    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
    }

    /*В апдейте контролируем отзеркаливание спрайта игрока.
     *Если игрок в радиусе атаки, то останавливаем врага.
     *Если нет - двигаем врага в направлении игрока.
     */
    private void Update()
    {
        if (PlayerTransform != null) SetTargetPoint(PlayerTransform.position);

        UpdateEnemySpriteFlip(enemyRB, enemySR, TargetPoint);

        if (enemyAttack.PlayerInRange)
        {
            MoveEnemy(Vector3.zero);
        }
        else
        {
            MoveEnemy(GetMovementDirection() * Speed);
        }
    }

    //Метод передает направление движения в velocity rigidbody врага.
    private void MoveEnemy(Vector3 Direction) => enemyRB.velocity = Direction;

    //Метод рассчитывает направление движение по отношению к местонахождению игрока.
    private Vector2 GetMovementDirection() => CalculateMovementDirection(new Vector2(TargetPoint.x, transform.position.y));
    #endregion
}
