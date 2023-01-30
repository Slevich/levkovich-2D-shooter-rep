using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class Boss : MonoBehaviour
{
    #region Переменные
    [Header("Audio source, which playing movement sounds.")]
    [SerializeField] private AudioSource movementAudioSource;
    [Header("Array of footsteps sounds.")]
    [SerializeField] private AudioClip[] footstepsSounds;
    [Header("Player character transform component.")]
    [SerializeField] private Transform playerTransform;
    [Header("Image, which represents current HP level of enemy.")]
    [SerializeField] private Image HPBar;
    [Header("Timeline, which plays, when enemy HP level falls down under 50%.")]
    [SerializeField] private PlayableDirector nextStageTimeline;
    [Header("Walking speed of enemy.")]
    [SerializeField] private float walkSpeed;
    [Header("The interval of time after which the enemy attacks.")]
    [SerializeField] private float attackInterval;
    [Header("Damage, which applies to player's health.")]
    [SerializeField] private float attackDamage;
    [Header("Variable, which represents that playar in melee attack range.")]
    public bool playerInRange;

    //Аниматор врага.
    private Animator bossAnim;
    //Rigidbody врага.
    private Rigidbody2D bossRB;
    //Sprite Renderer врага.
    private SpriteRenderer bossSR;
    //Компонент здоровья врага.
    private Health enemyHealth;
    //Переменная, хранящая номер аудио клипа.
    private int clipNumber;
    //Переменная, показывающая атакует ли враг.
    private bool isAttack;
    //Переменная, показывающая закончена ли атака врага.
    private bool attackEnded;
    //Переменная, хранящая номер текущего стейта врага.
    private float enemyState;
    //Постоянный переменные, хранящие номера стейтов.
    private const float IDLE_STATE = 0;
    private const float WALK_STATE = 1;
    private const float ATTACK_STATE = 2;
    //Таймер для обнуления интервала атаки врага.
    private float currentAttackInterval;
    #endregion

    #region Методы
    /// <summary>
    /// На старте получаем необходимые компоненты.
    /// Дефолтным стейтом является ходьба (1).
    /// Присваиваем таймеру для обнуления - интервал атаки босса.
    /// </summary>
    private void Start()
    {
        bossAnim = GetComponent<Animator>();
        bossRB = GetComponent<Rigidbody2D>();
        bossSR = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<Health>();
        ChangeBossState(1);
        currentAttackInterval = attackInterval;
    }

    /// <summary>
    /// В Update обновляем стейты врага.
    /// Обновляем уровень ХП врага.
    /// Обновляем поворот спрайта врага.
    /// Проверяем не упал ли уровень ХП для начала второй стадии.
    /// Если игрок в радиусе атаки - враг атакует.
    /// Если нет - враг идет до игрока.
    /// </summary>
    private void Update()
    {
        EnemyStates();
        UpdateHPLevel();
        UpdateSpriteFlip();
        NextStage();

        if (playerInRange)
        {
            if (attackEnded == false)
            {
                isAttack = true;
            }

            EnemyAttack();
            EnemyAttackToIdle();
        }
        else
        {
            isAttack = false;
            attackInterval = currentAttackInterval;
            ChangeBossState(1);
            EnemyMove();
        }
    }

    /// <summary>
    /// Метод переключает стейты врага
    /// в зависимости от его номера.
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
                bossAnim.SetBool("IsAttack", false);
                bossAnim.SetFloat("Speed", 1f);
                break;

            case ATTACK_STATE:
                bossAnim.SetFloat("Speed", 0f);
                bossAnim.SetBool("IsAttack", true);
                bossRB.velocity = Vector2.zero;
                break;
        }
    }

    /// <summary>
    /// Метод расситывает местонахождения игрока.
    /// Передает направление движения в велосити врага.
    /// </summary>
    private void EnemyMove()
    {
        Vector3 enemyTargetPosition = new Vector3(playerTransform.position.x, transform.position.y, 0);
        Vector3 heading = enemyTargetPosition - transform.position;
        bossRB.velocity = heading * walkSpeed;
    }

    /// <summary>
    /// Метод рассчитывает процент ХП врага.
    /// Передайт его в Image с HP Bar'ом.
    /// </summary>
    private void UpdateHPLevel()
    {
        float enemyHP = enemyHealth.GetCurrentHealthProcent();
        HPBar.fillAmount = enemyHP;
    }

    /// <summary>
    /// Метод поворачивает спрайт врага
    /// в зависимости от местоположения
    /// игрока.
    /// </summary>
    private void UpdateSpriteFlip()
    {
        if (playerTransform.position.x <= transform.position.x)
        {
            bossSR.flipX = true;
        }
        else
        {
            bossSR.flipX = false;
        }
    }

    /// <summary>
    /// Метод переключает состояние покоя
    /// в состояние атаки по истечению
    /// интервала атаки.
    /// </summary>
    private void EnemyAttackToIdle()
    {
        if (attackEnded)
        {
            isAttack = false;
            bossAnim.SetBool("IsAttack", false);
            ChangeBossState(0);
            attackInterval -= Time.deltaTime;

            if (attackInterval <= 0)
            {
                attackInterval = currentAttackInterval;
                isAttack = true;
                ChangeBossState(2);
                attackEnded = false;
            }
        }
    }

    /// <summary>
    /// Метод проверяет, атакует ли игрок.
    /// Если да, передает значение в параметр аниматора.
    /// Стейт босса меняется на атаку (2).
    /// </summary>
    private void EnemyAttack()
    {
        if (isAttack)
        {
            bossAnim.SetBool("IsAttack", true);
            ChangeBossState(2);
        }
    }

    /// <summary>
    /// Метод проверяет, в радиусе атаки ли игрок.
    /// Если да - вызывает метод нанесения урона.
    /// </summary>
    private void Damage()
    {
        if (playerInRange)
        {
            playerTransform.GetComponent<Health>().ToDamage(attackDamage);
        }
    }

    /// <summary>
    /// Метод вызывается в конце анимации атаки,
    /// чтобы прервать проигрывание анимации.
    /// </summary>
    private void AttackEnded()
    {
        attackEnded = true;
    }


    /// <summary>
    /// Метод меняет текущий стейт врага
    /// в зависимости от номера, переданного
    /// в параметр.
    /// </summary>
    /// <param name="stateNumber"></param>
    private void ChangeBossState(int stateNumber)
    {
        enemyState = stateNumber;
    }

    /// <summary>
    /// Метод проверяет, если текущее здоровья врага
    /// становится меньше или равно 50% от максимального,
    /// запускается таймлайн следующей стадии.
    /// </summary>
    private void NextStage()
    {
        if (enemyHealth.GetCurrentHealthProcent() <= 0.5f)
        {
            nextStageTimeline.Play();
        }
    }

    /// <summary>
    /// Метод генерирует число, 
    /// для проигрывания рандомного
    /// аудио клипа из массива.
    /// </summary>
    public void PlayFootStepSound()
    {
        clipNumber = Random.Range(0, footstepsSounds.Length);
        movementAudioSource.clip = footstepsSounds[clipNumber];
        movementAudioSource.Play();
    }
    #endregion
}
