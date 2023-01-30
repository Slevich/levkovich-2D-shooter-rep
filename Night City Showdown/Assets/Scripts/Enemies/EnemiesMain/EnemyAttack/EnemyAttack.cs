using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private GameObject playerGameObject;
    public GameObject PlayerGameObject { get { return playerGameObject; } set { playerGameObject = value; } }

    private bool playerInRange;
    public bool PlayerInRange { get { return playerInRange; } set { playerInRange = value; } }
}
