using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMeleEnemyAnimations : EnemyAnimation
{
    #region Поля
    //Компоненты врага.
    private Animator enemyAnim;
    private Rigidbody2D enemyRB;
    //Переменная, содержащая текущую скорость врага из Rigidbody.
    private float speed;
    #endregion

    #region Методы
    //Получаем компоненты.
    private void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    /*В апдейте передаем магнитуду velocity rigidbody.
     *Вызываем метод изменения состояний аниматора.
     */ 
    private void Update()
    {
        speed = enemyRB.velocity.magnitude;
        EnemyAnimStates();
    }

    //Метод переключает поведение аниматора в соответствии с текущим стейтом игрока.
    private void EnemyAnimStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.Movement:
                enemyAnim.SetBool("IsAttack", false);
                enemyAnim.SetFloat("Speed", speed);
                break;

            case EnemyStates.Attack:
                enemyAnim.SetBool("IsAttack", true);
                break;

            case EnemyStates.Death:
                enemyAnim.SetBool("IsDead", true);
                break;
        }
    }
    #endregion
}
