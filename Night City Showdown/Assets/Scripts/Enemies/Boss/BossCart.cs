using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BossCart : MonoBehaviour
{
    #region Переменные
    [Header("Player character transform component.")]
    [SerializeField] private Transform playerTransform;
    [Header("Image, which represents current HP level of enemy.")]
    [SerializeField] private Image HPBar;
    [Header("Timeline, which plays, when enemy is dead.")]
    [SerializeField] private PlayableDirector deathTimeline;
    [Header("Speed of enemy riding.")]
    [SerializeField] private float rideSpeed;
    [Header("Time, which enemy waiting after attack.")]
    [SerializeField] private float waitingTimer;
    [Header("Damage, which applies to player's health.")]
    [SerializeField] private float attackDamage;
    [Header("Force, which apply to players rigidbody.")]
    [SerializeField] private float pushForce;
    [Header("Points, which player earned for killing enemy.")]
    [SerializeField] private int pointsCost;

    //Аниматор врага.
    private Animator bossAnim;
    //Rigidbody врага.
    private Rigidbody2D bossRB;
    //Sprite renderer врага.
    private SpriteRenderer bossSR;
    //Компонент здоровья врага.
    private Health enemyHealth;
    //Направление движения врага.
    private float moveDirection;
    //Перемення, хранящая значение, жив ли враг.
    private bool isAlive;
    //Переменная, хранящая значение, двигается ли враг.
    public bool isMoving;
    //Переменная, хранящая значение с текущим номером стейта врага.
    private float enemyState;
    //Постоянные переменные, хранящие номера стейтов врага.
    private const float IDLE_STATE = 0;
    private const float WALK_STATE = 1;
    //Таймер для обнуления.
    private float currentWaitingTimer;
    //Переменная-выключатель начисления очков.
    private bool pointsEarned;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем компоненты.
    /// Дефолтный стейт - передвижение (1).
    /// Обновляем направление движения.
    /// Присваиваем в таймер для обнуления значения.
    /// Переключаем значение переменной движется ли враг в true.
    /// </summary>
    private void Start()
    {
        bossAnim = GetComponent<Animator>();
        bossRB = GetComponent<Rigidbody2D>();
        bossSR = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<Health>();
        ChangeBossState(0);
        UpdateMoveDirection();
        currentWaitingTimer = waitingTimer;
        isMoving = false;
        pointsEarned = false;
    }

    /// <summary>
    /// В Update обновляем уровень здоровья.
    /// Проверяем, не умер ли враг.
    /// Если враг жив, обновляем стейты врага.
    /// Если враг движется, перемещаем его.
    /// Если враг не движется, останавливаем его,
    /// обновляем направление движения, поворачиваем
    /// спрайт в направлении игрока.
    /// </summary>
    private void Update()
    {
        UpdateHPLevel();
        OnEnemyDeath();

        if (isAlive)
        {
            EnemyStates();
            
            if (isMoving)
            {
                EnemyMove();
            }
            else
            {
                StopBoss();
                UpdateMoveDirection();
                UpdateSpriteFlip();
            }
        }
    }

    /// <summary>
    /// Метод переключает стейты врага
    /// в ссответствии с его номером.
    /// </summary>
    private void EnemyStates()
    {
        switch (enemyState)
        {
            case IDLE_STATE:
                bossAnim.SetFloat("Speed", 0f);
                bossRB.velocity = Vector2.zero;
                break;

            case WALK_STATE:
                bossAnim.SetFloat("Speed", 1f);
                break;
        }
    }

    /// <summary>
    /// Метод меняет стейт на перемещение (1).
    /// Передаем в велосити новый вектор с направлением движения
    /// умноженном на скорость перемещения.
    /// </summary>
    private void EnemyMove()
    {
        ChangeBossState(1);
        bossRB.velocity = new Vector2(moveDirection, transform.position.y) * rideSpeed;
    }

    /// <summary>
    /// Метод рассчитывает процент текущего здоровья врага,
    /// при этом, так как это второй стейт, заполняем HP Bar
    /// в половину. Проверяем жив ли враг.
    /// </summary>
    private void UpdateHPLevel()
    {
        float enemyHP = enemyHealth.GetCurrentHealthProcent();
        HPBar.fillAmount = enemyHP / 2;
        isAlive = enemyHealth.IsAlive;
    }

    /// <summary>
    /// Метод останавливает врага.
    /// Меняем стейт на покой (0).
    /// Запускаем таймер ожидания,
    /// по истечении которого начинаем двигать врага,
    /// обнуляем таймер ожидания.
    /// </summary>
    public void StopBoss()
    {
        isMoving = false;
        bossRB.velocity = Vector2.zero;
        ChangeBossState(0);

        waitingTimer -= Time.deltaTime;

        if (waitingTimer <= 0)
        {
            isMoving = true;
            waitingTimer = currentWaitingTimer;
        }
    }

    /// <summary>
    /// Метод наносит урон и толкает игрока в направлении движения.
    /// </summary>
    /// <param name="playerRB"></param>
    public void Damage(Rigidbody2D playerRB)
    {
        playerTransform.GetComponent<Health>().ToDamage(attackDamage);
        playerRB.AddForce(new Vector2(moveDirection, 1f) * pushForce, ForceMode2D.Force);
    }

    /// <summary>
    /// Если враг умер, останавливаем его, добавляем в статистику данные.
    /// Запускам таймлан.
    /// </summary>
    private void OnEnemyDeath()
    {
        if (isAlive == false)
        {
            bossRB.velocity = Vector2.zero;
            if (pointsEarned == false)
            {
                playerTransform.GetComponent<MainCharUICounts>().pointsEarned += pointsCost;
                playerTransform.GetComponent<MainCharUICounts>().enemyKilled++;
                pointsEarned = true;
            }
            deathTimeline.Play();
        }
    }

    /// <summary>
    /// Метод поворачивает спрайт врага, 
    /// в зависимости от нахождения игрока.
    /// </summary>
    private void UpdateSpriteFlip()
    {
        if (playerTransform.position.x > transform.position.x)
        {
            bossSR.flipX = false;
        }
        else
        {
            bossSR.flipX = true;
        }
    }

    /// <summary>
    /// Метод обновляет направление движения врага,
    /// в зависимости от места нахождения игрока.
    /// </summary>
    private void UpdateMoveDirection()
    {
        if (playerTransform.position.x > transform.position.x)
        {
            moveDirection = 1;
        }
        else
        {
            moveDirection = -1;
        }
    }

    /// <summary>
    /// Метод меняет номер стейта врага.
    /// </summary>
    /// <param name="stateNumber"></param>
    private void ChangeBossState(int stateNumber)
    {
        enemyState = stateNumber;
    }

    /// <summary>
    /// Если враг уперся в границу,
    /// он перестает двигаться.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Border"))
        {
            isMoving = false;
        }
    }
    #endregion
}
