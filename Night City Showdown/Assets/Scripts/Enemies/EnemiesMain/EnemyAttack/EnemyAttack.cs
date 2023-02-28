using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Игровой объект с объектом игрока.
    private GameObject playerGameObject;
    //Свойство для получения или задания объекта игрока.
    public GameObject PlayerGameObject { get { return playerGameObject; } set { playerGameObject = value; } }
    //Переключатель, обозначающий в радиусе атаки ли игрок.
    private bool playerInRange;
    //Свойство для получения или задания переключателя в радиусе атаки ли игрок.
    public bool PlayerInRange { get { return playerInRange; } set { playerInRange = value; } }
}
