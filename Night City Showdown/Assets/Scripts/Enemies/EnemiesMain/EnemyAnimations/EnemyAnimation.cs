using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    //Экземпляр перечисления со стейтами врага.
    protected EnemyStates enemyStates = new EnemyStates();

    //Перечисление с наименованиями стейтов врага.
    public enum EnemyStates : int
    {
        Movement = 1,
        Attack = 2,
        Death = 3
    }

    /// <summary>
    /// Метод меняет текущий стейт игрока.
    /// </summary>
    /// <param name="EnemyState">Текущий анимационный стейт врага.</param>
    public void ChangeEnemyState(EnemyStates EnemyState)
    {
        enemyStates = EnemyState;
    }
}
