using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamAttack : EnemyAttack
{
    #region Поля
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float waitingTimer;
    [SerializeField] private float ramDamage;
    [SerializeField] private float pushForce;

    private Vector2 movementDirection;
    //Аниматор врага.
    private EnemyNoAttackAnimations bossAnim;
    //Rigidbody врага.
    private Rigidbody2D bossRB;
    //Sprite renderer врага.
    private SpriteRenderer bossSR;
    //Компонент здоровья врага.
    private EnemyHealth enemyHealth;
    //Переменная, обозначающая, столкнулся ли босс с преградой.
    private bool borderCollided;
    //Переменная, обозначающая, движется ли босс.
    private bool isMoving;
    //Переменная для перезарядки таймера ожидания.
    private float reloadWaitingTimer;
    #endregion

    #region Свойства
    public bool BorderCollided { set { borderCollided = value; } }
    public bool IsMoving { get { return isMoving; } }
    #endregion

    #region Методы
    /*Получаем компоненты на старте.
     *Стартовый анимационный стейт - движение.
     *Присваиваем таймеру для обнуления указанное значение.
     */
    private void Awake()
    {
        bossAnim = GetComponent<EnemyNoAttackAnimations>();
        bossRB = GetComponent<Rigidbody2D>();
        bossSR = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();
        bossAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);
        reloadWaitingTimer = waitingTimer;
    }

    /*Если враг находится в движении и он жив,
     *двигаемся в направлении игрока. Если же он
     *столкнулся с игроком или границей, обнуляем таймер ожидания.
     *Если враг не двигается, останавливаем врага, расчитываем
     *направление атаки врага. Также контролируем отзеркаливание
     *спрайта врага.
     */
    private void Update()
    {
        Debug.Log(borderCollided);

        if (CanRide() && enemyHealth.IsAlive)
        {
            isMoving = true;
            ToRamPlayer(movementDirection);

            if (borderCollided) waitingTimer = reloadWaitingTimer;
        }
        else
        {
            isMoving = false;
            borderCollided = false;
            bossRB.velocity = Vector2.zero;
            movementDirection = CalculateMovementDirection(); 
            UpdateSpriteFlip();
        }
    }

    //Метод передает направление движения в rigidbody velocity.
    private void ToRamPlayer(Vector2 movementDirection)
    {
        bossRB.velocity = movementDirection * movementSpeed;
    }

    /*Метод запускает таймер ожидания.
     *Если он истек, возвращает true,
     *если нет - false.
     */
    private bool CanRide()
    {
        waitingTimer -= Time.deltaTime;

        if (waitingTimer <= 0) return true;
        else return false;
    }

    //Метод отзеркаливает спрайт в зависимости от положения игрока относительно врага.
    private void UpdateSpriteFlip()
    {
        if (playerTransform.position.x > transform.position.x) bossSR.flipX = false;
        else bossSR.flipX = true;
    }

    /*Метод вызывает метод нанесения урона и игрового объекта игрока,
     *после чего добавляет силу к rigidbody игрока.
     */

    public void DamagePlayer(GameObject playerGameObject)
    {
        playerGameObject.GetComponent<Health>().ToDamage(ramDamage);
        playerTransform.gameObject.GetComponent<Rigidbody2D>().AddForce(CalculatePushDirection() * pushForce, ForceMode2D.Force);
    }

    //Метод расчитывает направления толчка игрока при ударе.
    private Vector2 CalculatePushDirection()
    {
        Vector2 heading = playerTransform.position - transform.position;
        float pushDistance = heading.magnitude;
        Vector2 pushDirection = heading / pushDistance;
        return pushDirection;
    }

    //Метод получает направление движения врага в зависимости от положения игрока относительно врага.
    private Vector2 CalculateMovementDirection()
    {
        Vector2 movementDirection;

        if (playerTransform.position.x > transform.position.x)
        {
            movementDirection = Vector2.right;
        }
        else
        {
            movementDirection = Vector2.left;
        }

        return movementDirection;
    }
    #endregion
}
