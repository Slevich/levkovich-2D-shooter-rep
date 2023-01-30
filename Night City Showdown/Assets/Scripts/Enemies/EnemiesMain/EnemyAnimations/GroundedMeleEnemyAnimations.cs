using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMeleEnemyAnimations : EnemyAnimation
{
    private Animator enemyAnim;
    private EnemyMovement enemyMovement;

    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        EnemyAnimStates();
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
}
