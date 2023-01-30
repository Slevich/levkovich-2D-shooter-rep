using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    protected EnemyStates enemyStates = new EnemyStates();

    public enum EnemyStates : int
    {
        Movement = 1,
        Attack = 2,
        Death = 3
    }

    public void ChangeEnemyState(EnemyStates EnemyState)
    {
        enemyStates = EnemyState;
    }
}
