using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Поля
    [Header("Borders of waiting until moving.")]
    [SerializeField] private float minWaitBorder;
    [SerializeField] private float maxWaitBorder;
    [Header("Speed of movement.")]
    [SerializeField] private float speed;
    [Header("The distance at which the enemy stops when reaching a point.")]
    [SerializeField] private float enemyStopDistance;
    [Header("Transform component of the player game object (Joy).")]
    [SerializeField] private Transform playerTransform;

    //Точка, к которой направляется враг.
    private Vector2 targetPoint;
    //Переключатель, обозначающий, движется ли враг.
    private bool isMoving;
    //Переменная, обозначающая достиг ли враг точки.
    protected bool enemyReachPoint;
    //Переключатель, обозначающий попала ли в триггер стена.
    private bool wallDetected;
    //Переключатель, обозначающий попал ли игрок в триггер.
    private bool playerDetected;
    //Переключатель, обозначающий сгенерирован ли таймер.
    private bool timerGenerated;
    //Время, которое ждет враг перед продолжением движения.
    private float waitingTimer;
    private bool isLookForward;
    protected float movementMagnitude;
    #endregion

    #region Свойства
    public Transform PlayerTransform { get { return playerTransform; } }
    public Vector2 TargetPoint { get { return targetPoint; } protected set { targetPoint = value; } }
    public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
    public bool PlayerDetected { get { return playerDetected; } set { playerDetected = value; } }
    public bool WallDetected { get { return wallDetected; } set { wallDetected = value; } }
    protected float Speed { get { return speed; } }
    protected bool IsEnemyReachPoint { get { return enemyReachPoint; } }
    public bool IsLookForward { get { return isLookForward; } }
    protected bool TimerGenerated { get { return timerGenerated; } set { timerGenerated = value; } }
    protected float MinWaitBorder { get { return minWaitBorder; } }
    protected float MaxWaitBorder { get { return maxWaitBorder; } }
    protected float WaitingTimer { get { return waitingTimer; } set { waitingTimer = value; } }
    public float MovementMagnitude { get { return movementMagnitude; } }
    #endregion

    #region Методы
    private void OnValidate()
    {
        if (speed < 0)
        {
            speed = 0;
        }

        if (minWaitBorder < 0)
        {
            minWaitBorder = 0;
        }
        else if (maxWaitBorder < 0)
        {
            maxWaitBorder = 0;
        }
        else if (maxWaitBorder < minWaitBorder)
        {
            maxWaitBorder = minWaitBorder + 1f;
        }
    }

    protected void UpdateEnemySpriteFlip(Rigidbody2D enemyRB, SpriteRenderer enemySR)
    {
        if (enemyRB.velocity.x > 0)
        {
            enemySR.flipX = false;
        }
        else if (enemyRB.velocity.x < 0)
        {
            enemySR.flipX = true;
        }
    }

    protected void GetEnemyLookDirection(SpriteRenderer enemySR)
    {
        if (enemySR.flipX == false) isLookForward = true;
        else isLookForward = false;
    }

    protected virtual void GenerateTargetPoint(SpriteRenderer enemySR, CapsuleCollider2D enemyRangeTrigger, CircleCollider2D enemyFindingTrigger)
    {
        if (wallDetected == false)
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= 0.5) SetTargetPoint(new Vector2(Random.Range(transform.position.x + enemyRangeTrigger.size.x, 
                                                                            transform.position.x + enemyFindingTrigger.radius), 
                                                                            transform.position.y));
            else SetTargetPoint(new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, 
                                                        transform.position.x - enemyRangeTrigger.size.x), 
                                                        transform.position.y));
            timerGenerated = false;
            wallDetected = false;
            isMoving = true;
        }
        else
        {
            if (enemySR.flipX)
            {
                SetTargetPoint(new Vector2(Random.Range(transform.position.x + enemyRangeTrigger.size.x, 
                                                        transform.position.x + enemyFindingTrigger.radius), 
                                                        transform.position.y));
                timerGenerated = false;
                wallDetected = false;
                isMoving = true;
            }
            else
            {
                SetTargetPoint(new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, 
                                                        transform.position.x - enemyRangeTrigger.size.x), 
                                                        transform.position.y));
                timerGenerated = false;
                wallDetected = false;
                isMoving = true;
            }
        }
    }

    protected virtual void EnemyGoesToPosition(Vector2 targetPoint, EnemyChecks enemyChecks, Rigidbody2D enemyRB, EnemyAnimation enemyAnim)
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
        MoveCharacter(targetPoint, enemyChecks, enemyRB);
        
        if (playerDetected == false)
        {
            EnemyReachPoint();

            if (enemyReachPoint) StopEnemy(enemyAnim, enemyRB);
        }
    }

    protected void EnemyReachPoint()
    {
        if (Vector2.Distance(transform.position, targetPoint) <= enemyStopDistance)
        {
            enemyReachPoint = true;
        }
        else
        {
            enemyReachPoint = false;
        }
    }

    protected void StopEnemy(EnemyAnimation enemyAnim, Rigidbody2D enemyRB)
    {
        if (isMoving)
        {
            enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
            enemyRB.velocity = Vector2.zero;
            isMoving = false;
        }
    }

    protected Vector2 CalculateMovementDirection(Vector2 targetPoint)
    {
        Vector2 enemyTarget = targetPoint;
        Vector2 heading = enemyTarget - new Vector2(transform.position.x, transform.position.y);
        float targetDistance = heading.magnitude;
        Vector2 movingDirection = heading / targetDistance;
        return movingDirection;
    }

    protected virtual void MoveCharacter(Vector2 targetPoint, EnemyChecks enemyChecks, Rigidbody2D enemyRB)
    {
        enemyRB.velocity = CalculateMovementDirection(targetPoint) * speed;
    }

    protected virtual void WaitingTimerBeforeMovement(EnemyAnimation enemyAnim, SpriteRenderer enemySR, CapsuleCollider2D enemyRangeTrigger, CircleCollider2D enemyFindingTrigger)
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);

        if (timerGenerated == false)
        {
            waitingTimer = Random.Range(minWaitBorder, maxWaitBorder);
            timerGenerated = true;
        }

        waitingTimer -= Time.deltaTime;

        if (waitingTimer < 0)
        {
            GenerateTargetPoint(enemySR, enemyRangeTrigger, enemyFindingTrigger);
        }
    }

    protected void SetTargetPoint(Vector2 NewTargetPoint) => targetPoint = NewTargetPoint;
    #endregion
}
