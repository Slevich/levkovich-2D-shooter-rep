using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemyMovement : EnemyMovement
{
    #region Поля
    //Коллайдеры врага.
    private CircleCollider2D enemyFindingTrigger;
    private CapsuleCollider2D enemyRangeTrigger;
    //Компоненты врага.
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;
    private EnemyChecks enemyChecks;
    private EnemyAnimation enemyAnim;
    #endregion

    #region Методы
    //Получаем компоненты и коллайдеры врага.
    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyChecks = GetComponent<EnemyChecks>();
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyFindingTrigger = enemyChecks.EnemyCircleTrigger;
        enemyRangeTrigger = enemyChecks.EnemyCheckTrigger;
    }

    /*В зависимости от того, замечен ли враг:
     * 1) если замечен:
     * a) если игрок на земле и движется - направляем его к точке;
     * б) если игрок падает - его направление движения вниз;
     * в) в других случаях - враг стоит на месте;
     * 2) если не замечен:
     * а) если враг двигается - направляем его к точке;
     * б) если враг не двигается - ждет таймер.
     */
    private void Update()
    {
        Debug.DrawLine(transform.position, TargetPoint, Color.blue);
        UpdateEnemySpriteFlip(enemyRB, enemySR, TargetPoint);
        GetEnemyLookDirection(enemySR);

        if (PlayerDetected)
        {
            if (enemyChecks.IsGroundForward && enemyChecks.IsGroundBackward && IsMoving)
            {
                EnemyGoesToPosition(TargetPoint, enemyChecks, enemyRB, enemyAnim);
            }
            else if (enemyChecks.IsFalling)
            {
                enemyRB.velocity = Vector2.down * Mathf.PI;
            }
            else
            {
                enemyRB.velocity = Vector2.zero;
            }
        }
        else
        {
            if (IsMoving)
            {
                EnemyGoesToPosition(TargetPoint, enemyChecks, enemyRB, enemyAnim);
            }
            else
            {
                WaitingTimerBeforeMovement(enemyAnim, enemySR, enemyRangeTrigger, enemyFindingTrigger);
            }
        }
    }

    //Метод в зависимости от того замечен игрок, задает направление движения и передает его в velocity rigidbody.
    protected override void MoveCharacter(Vector2 targetPoint, EnemyChecks enemyChecks, Rigidbody2D enemyRB)
    {
        Vector2 movementDirection;

        if (PlayerDetected)
        {
            movementDirection = SetMovementDirectionDetected();
        }
        else
        {
            movementDirection = SetMovementDirectionNonDetected();
        }

        enemyRB.velocity = movementDirection * Speed;
    }

    //Метод двигает врага к точке и проверяет, достиг он цели или уперся ли в стену.
    protected void EnemyGoesToPosition(Vector2 targetPoint, EnemyChecks enemyChecks, Rigidbody2D enemyRB, EnemyAnimation enemyAnim)
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
        MoveCharacter(targetPoint, enemyChecks, enemyRB);

        if (PlayerDetected == false)
        {
            EnemyReachPoint();

            if (enemyReachPoint || WallDetected) StopEnemy(enemyAnim, enemyRB);
        }
    }

    /*В зависимости от того, падает ли игрок, или замечен ли враг,
     *или отсутствует ли земля по сторонам, задааем точку и (или) расчитываем направление движения.
     */
    private Vector2 SetMovementDirectionNonDetected()
    {
        Vector2 movementDirection = Vector2.zero;

        if (enemyChecks.IsFalling)
        {
            movementDirection = Vector2.down;
        }
        else if (IsGroundOrEnemyBothSides())
        {
            movementDirection = Vector2.zero;
        }
        else if (IsGroundOrEnemyForward())
        {
            SetTargetPoint(new Vector2(transform.position.x - enemyRangeTrigger.size.x, transform.position.y));
            movementDirection = CalculateMovementDirection(TargetPoint);
        }
        else if (IsGroundOrEnemyBackward())
        {
            SetTargetPoint(new Vector2(transform.position.x + enemyRangeTrigger.size.x, transform.position.y));
            movementDirection = CalculateMovementDirection(TargetPoint);
        }
        else
        {
            movementDirection = CalculateMovementDirection(TargetPoint);
        }
        return movementDirection;
    }

    /*В зависимости от того, падает ли игрок, или замечен ли враг,
     *или отсутствует ли земля по сторонам, задааем точку и (или) расчитываем направление движения.
     */
    private Vector2 SetMovementDirectionDetected()
    {
        Vector2 movementDirection = Vector2.zero;

        if (enemyChecks.IsFalling)
        {
            movementDirection = Vector2.down;
        }
        else if (IsGroundOrEnemyBothSides())
        {
            movementDirection = Vector2.zero;
        }
        else if (IsGroundOrEnemyForward())
        {
            if (PlayerIsForward())
            {
                movementDirection = Vector2.zero;
            }
            else
            {
                SetTargetPoint(new Vector2(PlayerTransform.position.x, transform.position.y));
                movementDirection = CalculateMovementDirection(TargetPoint);
            }
        }
        else if (IsGroundOrEnemyBackward())
        {
            if (PlayerIsForward())
            {
                SetTargetPoint(new Vector2(PlayerTransform.position.x, transform.position.y));
                movementDirection = CalculateMovementDirection(TargetPoint);
            }
            else
            {
                movementDirection = Vector2.zero;
            }
        }
        else
        {
            SetTargetPoint(new Vector2(PlayerTransform.position.x, transform.position.y));
            movementDirection = CalculateMovementDirection(TargetPoint);
        }
        return movementDirection;
    }

    //Метод возвращает bool (впереди игрок или нет) в зависимости от положения игрок относительно врага.
    private bool PlayerIsForward() => transform.position.x < PlayerTransform.position.x;
    //Если нет земли спереди или есть враг спереди, возвращает true.
    private bool IsGroundOrEnemyForward() => enemyChecks.IsGroundForward == false || enemyChecks.IsEnemyForward;
    //Если нет земли сзади или есть враг сзади, возвращает true.
    private bool IsGroundOrEnemyBackward() => enemyChecks.IsGroundBackward == false || enemyChecks.IsEnemyBackward;
    //Если нет земли спереди и сзади или есть враг спереди и сзади, возвращает true.
    private bool IsGroundOrEnemyBothSides() => (enemyChecks.IsGroundForward == false && enemyChecks.IsGroundBackward == false)
                                               || (enemyChecks.IsEnemyForward && enemyChecks.IsEnemyBackward);
    #endregion
}
