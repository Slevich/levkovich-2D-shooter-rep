using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    [SerializeField] private float meleeDamage;
    [SerializeField] private float attackInterval;

    private bool isAttack;
    private float reloadAttackInterval;
    private EnemyAnimation enemyAnim;

    private void Start()
    {
        enemyAnim = GetComponent<EnemyAnimation>();
        reloadAttackInterval = attackInterval;
    }

    private void Update()
    {
        if (PlayerInRange)
        {
            EnemyMeleeHit();
        }
    }

    private void EnemyMeleeHit()
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
        attackInterval -= Time.deltaTime;

        if (attackInterval <= 0)
        {
            if (isAttack) EnemyChangeOnAttackState();
            else attackInterval = reloadAttackInterval;
        }
        else isAttack = true;
    }

    private void EnemyChangeOnAttackState()
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Attack);
    }

    public void DamagePlayerInMelee()
    {
        if (PlayerInRange && PlayerGameObject != null)
        {
            PlayerGameObject.GetComponent<Health>().ToDamage(meleeDamage);
        }
    }

    public void StopAttack()
    {
        isAttack = false;
    }
}
