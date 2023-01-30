using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNoAttackAnimations : EnemyAnimation
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
                enemyAnim.SetFloat("Speed", enemyMovement.MovementMagnitude);
                break;
            case EnemyStates.Death:
                enemyAnim.SetBool("IsDead", true);
                break;
        }
    }
}
