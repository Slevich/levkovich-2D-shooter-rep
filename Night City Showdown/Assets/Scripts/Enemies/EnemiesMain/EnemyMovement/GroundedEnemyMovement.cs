using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemyMovement : EnemyMovement
{
    private CircleCollider2D enemyFindingTrigger;
    private CapsuleCollider2D enemyRangeTrigger;
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;
    private EnemyChecks enemyChecks;
    private EnemyAnimation enemyAnim;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyChecks = GetComponent<EnemyChecks>();
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyFindingTrigger = enemyChecks.EnemyCircleTrigger;
        enemyRangeTrigger = enemyChecks.EnemyCheckTrigger;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, TargetPoint, Color.blue);
        UpdateEnemySpriteFlip(enemyRB, enemySR);
        GetEnemyLookDirection(enemySR);
        movementMagnitude = GetMovementMagnitude();

        if (PlayerDetected)
        {
            if (enemyChecks.IsGroundForward && enemyChecks.IsGroundBackward && IsMoving)
            {
                EnemyGoesToPosition(new Vector2(PlayerTransform.position.x, transform.position.y), enemyChecks, enemyRB, enemyAnim);
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

    protected override void MoveCharacter(Vector2 targetPoint, EnemyChecks enemyChecks, Rigidbody2D enemyRB)
    {
        Vector2 movementDirection;

        if (enemyChecks.IsFalling)
        {
            SetTargetPoint(new Vector2(transform.position.x, transform.position.y - 1));
        }
        else if ((enemyChecks.IsGroundForward == false && enemyChecks.IsGroundBackward == false) || (enemyChecks.IsEnemyForward && enemyChecks.IsEnemyBackward))
        {
            SetTargetPoint(new Vector2(transform.position.x, transform.position.y));
        }
        else if (enemyChecks.IsGroundForward == false || enemyChecks.IsEnemyForward)
        {
            SetTargetPoint(new Vector2(transform.position.x - enemyRangeTrigger.size.x, transform.position.y));
        }
        else if (enemyChecks.IsGroundBackward == false || enemyChecks.IsEnemyBackward)
        {
            SetTargetPoint(new Vector2(transform.position.x + enemyRangeTrigger.size.x, transform.position.y));
        }

        movementDirection = CalculateMovementDirection(targetPoint);
        enemyRB.velocity = movementDirection * Speed;
    }

    protected override void EnemyGoesToPosition(Vector2 targetPoint, EnemyChecks enemyChecks, Rigidbody2D enemyRB, EnemyAnimation enemyAnim)
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
        MoveCharacter(targetPoint, enemyChecks, enemyRB);

        if (PlayerDetected == false)
        {
            EnemyReachPoint();

            if (enemyReachPoint || WallDetected) StopEnemy(enemyAnim, enemyRB);
        }
    }

    private bool CheckIsPlayerInLowground()
    {
        return transform.position.x > (PlayerTransform.position.x + enemyRangeTrigger.size.x);
    }


    protected float GetMovementMagnitude()
    {
        return enemyRB.velocity.magnitude;
    }
}
