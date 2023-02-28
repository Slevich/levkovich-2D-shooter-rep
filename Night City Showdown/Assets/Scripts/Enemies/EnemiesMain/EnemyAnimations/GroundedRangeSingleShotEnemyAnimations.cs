using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedRangeSingleShotEnemyAnimations : EnemyAnimation
{
    #region Поля
    //Компоненты врага.
    private Animator enemyAnim;
    private EnemySingleShotAttack enemyAttack;
    private Rigidbody2D enemyRB;
    #endregion

    #region Методы
    //Получаем компоненты.
    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemySingleShotAttack>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    /*В апдейте вызываем метод изменения состояний аниматора.
     *Если игрок в радиусе атаки, обновляем направление атаки врага.
     */ 
    private void Update()
    {
        EnemyAnimStates();

        if (enemyAttack.PlayerInRange)
        {
            UpdateAnimatorAttackDirection();
        }
    }

    //Метод переключает поведение аниматора в соответствии с текущим стейтом игрока.
    private void EnemyAnimStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.Movement:
                enemyAnim.SetBool("IsAttack", false);
                enemyAnim.SetFloat("Speed", enemyRB.velocity.magnitude);
                break;

            case EnemyStates.Attack:
                enemyAnim.SetBool("IsAttack", true);
                break;

            case EnemyStates.Death:
                enemyAnim.SetBool("IsDead", true);
                break;
        }
    }

    //Метод передает направление атаки врага в аниматор.
    private void UpdateAnimatorAttackDirection()
    {
        enemyAnim.SetFloat("AttackDirection", enemyAttack.AttackDirection);
    }
    #endregion
}
