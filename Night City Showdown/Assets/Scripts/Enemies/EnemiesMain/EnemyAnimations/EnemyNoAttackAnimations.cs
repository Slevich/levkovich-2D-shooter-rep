using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNoAttackAnimations : EnemyAnimation
{
    #region Поля
    //Компоненты врага.
    private Animator enemyAnim;
    private Rigidbody2D enemyRB;
    #endregion

    #region Методы
    //Получаем компоненты врага.
    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    //В апдейте вызываем метод со стейтами игрока.
    private void Update()
    {
        EnemyAnimStates();
    }

    //Метод переключает поведение аниматора в соответствии с текущим стейтом игрока.
    private void EnemyAnimStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.Movement:
                enemyAnim.SetFloat("Speed", enemyRB.velocity.magnitude);
                break;
            case EnemyStates.Death:
                enemyAnim.SetBool("IsDead", true);
                break;
        }
    }
    #endregion
}
