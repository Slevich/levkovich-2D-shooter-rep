using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecks : MonoBehaviour
{
    #region Переменные
    [SerializeField] private float groundRayDistance;
    [SerializeField] private float sidesRayDistance;
    [SerializeField] private float voidRayDistance;
    [SerializeField] private Vector2 backwardVoidRayDirection;
    [SerializeField] private Vector2 forwardVoidRayDirection;
    [Header("Layers masks.")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CircleCollider2D enemyCircleTrigger;
    [SerializeField] private CapsuleCollider2D enemyGroundTrigger;
    [SerializeField] private CapsuleCollider2D enemyCheckTrigger;

    private EnemyMovement enemyMovement;
    //Переключатель, обозначающий падает ли враг.
    private bool isFalling;
    //Переключатель, обозначающий замечен ли другой враг спереди.
    private bool isEnemyForward;
    //Переключатель, обозначающий замечен ли другой враг сзади.
    private bool isEnemyBackward;
    //Переключатель, обозначающий находится ли земля спереди.
    private bool isGroundForward;
    //Переключатель, обозначающий находится ли земля сзади.
    private bool isGroundBackward;

    public CircleCollider2D EnemyCircleTrigger { get { return enemyCircleTrigger; } }
    public CapsuleCollider2D EnemyCheckTrigger { get { return enemyCheckTrigger; } }
    public bool IsFalling { get { return isFalling; } }
    public bool IsEnemyForward { get { return isEnemyForward; } }
    public bool IsEnemyBackward { get { return isEnemyBackward; } }
    public bool IsGroundForward { get { return isGroundForward; } }
    public bool IsGroundBackward { get { return isGroundBackward; } }

    #endregion

    #region Методы
    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        GroundCheck();
        CheckVoid();
        //Debug.Log($"IsGroundForward: {isGroundForward}");
        //Debug.Log($"IsGroundBackward: {isGroundBackward}");
        //Debug.Log($"IsFalling: {isFalling}");
        Debug.Log($"IsEnemyForward: {isEnemyForward}");
        Debug.Log($"IsEnemyBackward: {isEnemyBackward}");
        Debug.DrawLine(enemyCircleTrigger.transform.position,
                       enemyCircleTrigger.transform.position + (new Vector3(forwardVoidRayDirection.x, forwardVoidRayDirection.y, 0) * voidRayDistance), 
                       Color.magenta);
        Debug.DrawLine(enemyCircleTrigger.transform.position,
                       enemyCircleTrigger.transform.position + (new Vector3(backwardVoidRayDirection.x, backwardVoidRayDirection.y, 0) * voidRayDistance), 
                       Color.magenta);
        Debug.DrawLine(new Vector2(enemyCheckTrigger.transform.position.x + (enemyCheckTrigger.size.x / 2), enemyCheckTrigger.transform.position.y),
                       new Vector2(enemyCheckTrigger.transform.position.x + (enemyCheckTrigger.size.x / 2) + sidesRayDistance, enemyCheckTrigger.transform.position.y),
                       Color.red);
        Debug.DrawLine(new Vector2(enemyCheckTrigger.transform.position.x - (enemyCheckTrigger.size.x / 2), enemyCheckTrigger.transform.position.y),
                       new Vector2(enemyCheckTrigger.transform.position.x - (enemyCheckTrigger.size.x / 2) - sidesRayDistance, enemyCheckTrigger.transform.position.y),
                       Color.red);

        if (enemyMovement.IsMoving)
        {
            AnotherEnemyCheck();
        }
    }

    private void GroundCheck()
    {
        isFalling = !Physics2D.CapsuleCast(enemyGroundTrigger.transform.position, enemyGroundTrigger.size, enemyGroundTrigger.direction, 0, Vector2.zero, 0, groundMask);
    }

    private void AnotherEnemyCheck()
    {
        isEnemyForward = Physics2D.Raycast(new Vector2(enemyCheckTrigger.transform.position.x + (enemyCheckTrigger.size.x / 2),
                                                       enemyCheckTrigger.transform.position.y),
                                                       Vector2.right, sidesRayDistance, enemyMask);
        isEnemyBackward = Physics2D.Raycast(new Vector2(enemyCheckTrigger.transform.position.x - (enemyCheckTrigger.size.x / 2),
                                                        enemyCheckTrigger.transform.position.y),
                                                        Vector2.left, sidesRayDistance, enemyMask);
    }

    private void CheckVoid()
    {
        isGroundForward = Physics2D.Raycast(enemyCircleTrigger.transform.position, 
                                            forwardVoidRayDirection, 
                                            voidRayDistance, 
                                            groundMask);
        isGroundBackward = Physics2D.Raycast(enemyCircleTrigger.transform.position, 
                                             backwardVoidRayDirection, 
                                             voidRayDistance, 
                                             groundMask);
    }
    #endregion
}
