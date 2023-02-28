using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthDowngrader : MonoBehaviour
{
    #region Поля
    //Компонент со здоровьем врага.
    private EnemyHealth enemyHealth;
    #endregion

    #region Методы
    /*На старте получаем компонент здоровья и занижаем максимальное здоровье
     *врага в половину.
     */
    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.ToDamage(enemyHealth.MaxHealth / 2);
    }
    #endregion
}
