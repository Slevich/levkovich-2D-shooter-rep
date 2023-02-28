using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemyMovement : EnemyMovement
{
    #region Поля
    //Максимальная позиция по Y, за которую враг не может вылететь.
    private float maxYPosition;
    //Коллайдеры врага.
    private CircleCollider2D enemyFindingTrigger;
    private CapsuleCollider2D enemyRangeTrigger;
    //Компоненты врага.
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;
    private EnemyAnimation enemyAnim;
    #endregion

    #region Методы
    //На старте получаем компоненты врага. Передаем максимальную позицию по Y.
    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<EnemyAnimation>();
        enemyFindingTrigger = GetComponentInChildren<CircleCollider2D>();
        enemyRangeTrigger = GetComponentInChildren<CapsuleCollider2D>();
        maxYPosition = transform.position.y + enemyFindingTrigger.radius;
    }

    /*В апдейте поварачиваем спрайт врага.
     * Если игрок замечен врагом:
     * 1) если враг двигается - направляется к игроку;
     * 2) если враг не двигается - останавливаем его.
     * Если игрок не замечен врагом:
     * 1) если враг двигается - он двигается к выбранной точке;
     * 2) если враг не двигается - враг ждем перед движением.
     */
    private void Update()
    {
        Debug.DrawLine(transform.position, TargetPoint, Color.blue);
        UpdateEnemySpriteFlip(enemyRB, enemySR, TargetPoint);

        if (PlayerDetected)
        {
            if (IsMoving)
            {
                EnemyGoesToPosition(PlayerTransform.position);
            }
            else
            {
                enemyRB.velocity = Vector2.zero;
            }
        }
        else
        {
            if (IsMoving)
            {
                EnemyGoesToPosition(TargetPoint);
            }
            else
            {
                WaitingTimerBeforeMovement(enemyRangeTrigger, enemyFindingTrigger);
            }
        }
    }

    /*Меняем анимационный стейт врага на движение.
     *Передаем в velocity rigidbody врага направление движения к точке.
     *Если игрок не замечен, проверяем дошел ли враг до точки.
     */
    private void EnemyGoesToPosition(Vector2 targetPoint)
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);

        enemyRB.velocity = CalculateMovementDirection(targetPoint) * Speed;

        if (PlayerDetected == false)
        {
            EnemyReachPoint();

            if (IsEnemyReachPoint) StopEnemy(enemyAnim, enemyRB);
        }
    }

    /*Генерируем число от 0 и 1.
     *В зависимости от числа, генерируем точку слева или справа.
     *Переключаем, что враг двигается в true, а сгенерирован ли таймер - false.
     */
    protected override void GenerateTargetPoint(SpriteRenderer enemySR, CapsuleCollider2D enemyRangeTrigger, CircleCollider2D enemyFindingTrigger)
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber <= 0.5) SetTargetPoint(new Vector2(Random.Range(transform.position.x + enemyRangeTrigger.size.x,
                                                                        transform.position.x + enemyFindingTrigger.radius),
                                                           Random.Range(transform.position.y, maxYPosition)));
        else SetTargetPoint(new Vector2(Random.Range(transform.position.x - enemyFindingTrigger.radius, transform.position.x - enemyRangeTrigger.size.x),
                                                     Random.Range(transform.position.y - enemyFindingTrigger.radius,
                                                     transform.position.y)));
        IsMoving = true;
        TimerGenerated = false;
    }

    /*Меняем анимационный стейт на движение.
     *Если таймер ожидания не сгенерирован - генерируем его.
     *Запускаем таймер ожидания. Когда таймер прошел,
     *генерируем новую точку движения.
     */
    private void WaitingTimerBeforeMovement(CapsuleCollider2D enemyRangeTrigger, CircleCollider2D enemyFindingTrigger)
    {
        enemyAnim.ChangeEnemyState(EnemyAnimation.EnemyStates.Movement);

        if (TimerGenerated == false)
        {
            WaitingTimer = Random.Range(MinWaitBorder, MaxWaitBorder);
            TimerGenerated = true;
        }

        WaitingTimer -= Time.deltaTime;

        if (WaitingTimer < 0)
        {
            GenerateTargetPoint(enemySR, enemyRangeTrigger, enemyFindingTrigger);
        }
    }
    #endregion
}
