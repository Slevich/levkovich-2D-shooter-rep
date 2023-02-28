using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecks : MonoBehaviour
{
    #region Переменные
    [Header("Distance of the ground check ray.")]
    [SerializeField] private float groundRayDistance;
    [Header("Distance of the sides check ray.")]
    [SerializeField] private float sidesRayDistance;
    [Header("Distance of the void check ray.")]
    [SerializeField] private float voidRayDistance;
    [SerializeField] private Vector2 backwardVoidRayDirection;
    [SerializeField] private Vector2 forwardVoidRayDirection;
    [Header("Layers masks.")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask enemyMask;
    [Header("Player colliders.")]
    [SerializeField] private CircleCollider2D enemyCircleTrigger;
    [SerializeField] private CapsuleCollider2D enemyGroundTrigger;
    [SerializeField] private CapsuleCollider2D enemyCheckTrigger;

    //Компонент с передвижением врага
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
    //На старте получаем компонент с движением игрока.
    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    /*В апдейте вызываем методы проверок.
     *Если игрок двигается, еще и проверяем
     *нет ли поблизости других врагов.
     */
    private void Update()
    {
        GroundCheck();
        CheckVoid();
        Debug.DrawLine(enemyCircleTrigger.transform.position,
                       enemyCircleTrigger.transform.position + (new Vector3(forwardVoidRayDirection.x, forwardVoidRayDirection.y, 0) * voidRayDistance), 
                       Color.magenta);
        Debug.DrawLine(enemyCircleTrigger.transform.position,
                       enemyCircleTrigger.transform.position + (new Vector3(backwardVoidRayDirection.x, backwardVoidRayDirection.y, 0) * voidRayDistance), 
                       Color.magenta);
        Debug.DrawLine(new Vector2(enemyCheckTrigger.transform.position.x + enemyCheckTrigger.size.x, enemyCheckTrigger.transform.position.y),
                       new Vector2(enemyCheckTrigger.transform.position.x + enemyCheckTrigger.size.x + sidesRayDistance, enemyCheckTrigger.transform.position.y),
                       Color.red);
        Debug.DrawLine(new Vector2(enemyCheckTrigger.transform.position.x - enemyCheckTrigger.size.x, enemyCheckTrigger.transform.position.y),
                       new Vector2(enemyCheckTrigger.transform.position.x - enemyCheckTrigger.size.x - sidesRayDistance, enemyCheckTrigger.transform.position.y),
                       Color.red);

        if (enemyMovement.IsMoving)
        {
            AnotherEnemyCheck();
        }
    }

    //Метод передает результат каста капсуля для проверки есть ли земля под ногами врага.
    private void GroundCheck()
    {
        isFalling = !Physics2D.CapsuleCast(enemyGroundTrigger.transform.position, enemyGroundTrigger.size, enemyGroundTrigger.direction, 0, Vector2.zero, 0, groundMask);
    }

    //Метод кидает два луча вправо и влево для проверки, есть ли другие враги поблизости.
    private void AnotherEnemyCheck()
    {
        isEnemyForward = Physics2D.Raycast(new Vector2(enemyCheckTrigger.transform.position.x + enemyCheckTrigger.size.x,
                                                       enemyCheckTrigger.transform.position.y),
                                                       Vector2.right, sidesRayDistance, enemyMask);
        isEnemyBackward = Physics2D.Raycast(new Vector2(enemyCheckTrigger.transform.position.x - enemyCheckTrigger.size.x,
                                                        enemyCheckTrigger.transform.position.y),
                                                        Vector2.left, sidesRayDistance, enemyMask);
    }

    //Метод кидат лучи под углом по сторонам, чтобы проверить имеется ли земля справа или слева.
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
