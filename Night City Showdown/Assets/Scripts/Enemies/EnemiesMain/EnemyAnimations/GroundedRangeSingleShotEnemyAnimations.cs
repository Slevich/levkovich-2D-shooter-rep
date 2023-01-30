using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedRangeSingleShotEnemyAnimations : EnemyAnimation
{
    private Animator enemyAnim;
    private EnemySingleShotAttack enemyAttack;
    private EnemyMovement enemyMovement;

    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemySingleShotAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        EnemyAnimStates();

        if (enemyAttack.PlayerInRange)
        {
            UpdateAnimatorAttackDirection();
        }
    }

    private void EnemyAnimStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.Movement:
                enemyAnim.SetBool("IsAttack", false);
                enemyAnim.SetFloat("Speed", enemyMovement.MovementMagnitude);
                break;

            case EnemyStates.Attack:
                enemyAnim.SetBool("IsAttack", true);
                break;

            case EnemyStates.Death:
                enemyAnim.SetBool("IsDead", true);
                break;
        }
    }

    private void UpdateAnimatorAttackDirection()
    {
        enemyAnim.SetFloat("AttackDirection", enemyAttack.AttackDirection);
    }
}
