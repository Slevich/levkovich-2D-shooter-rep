using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharMovement : MonoBehaviour
{
    #region Поля
    [Header("Value of movement speed of character.")]
    [SerializeField] private float movementSpeed;
    [Header("Value of jump force modifier.")]
    [SerializeField] private float jumpForce;
    [Header("Value of double jump force modifier.")]
    [SerializeField] private float doubleJumpModifier;
    [Header("Childer capsule collider used to check ground and enemies.")]
    [SerializeField] private CapsuleCollider2D groundCheckCapsule;
    [Header("Layer mask - 'Ground'.")]
    [SerializeField] private LayerMask groundMask;
    [Header("Layer mask - 'Enemy'.")]
    [SerializeField] private LayerMask enemyMask;
    [Header("Variable, which represent - is in air character now or not.")]
    public bool isInAir;
    [Header("Variable, which represent - character is jumping now or not.")]
    public bool isJumping;
    [Header("Variable, which represent - character is double jumping now or not.")]
    public bool isDoubleJumping;
    [Header("Variable, which containts permission to move.")]
    public bool canMove;
    [HideInInspector]
    public float jumpPosition;
    [HideInInspector]
    public float doubleJumpPosition;

    //Переменная, содержащая референс на компонент с пользовательским инпутом.
    private Project.Inputs.MainCharInput playerInput;
    //Переменная, содержащая референс на компонент со звуками персонажа.
    private MainCharSounds playerSounds;
    //Переменная, содержащая референс на компонент со здоровьем персонажа.
    private Health playerHealth;
    //Переменная, содержащая референс на компонент с пользовательским Rigidbody2D.
    private Rigidbody2D playerRB;
    //Переменная, отражающая количество прыжков персонажа, для отслеживания двойного прыжка.
    private int numberOfJumps;
    //Капсуль коллайдер игрока.
    private CapsuleCollider2D playerCollider;
    //Переменная, хранящая размеры колллайдера игрока.
    private Vector2 defaultColliderSize;
    //Значение, на которое уменьшается коллайдер игрока при двойном прыжке.
    private float colliderDifference = 0.1f;
    #endregion

    #region Свойства

    #endregion

    #region Методы
    /* На старте получаем компоненты.
     * Переводим переменные, отвечающие 
     * за нахождение в воздухе и прыжки в положение false.
     * Даем разрешение на перемещение.
     * Обнуляем количество прыжков.
     */
    private void Start()
    {
        playerInput = GetComponent<Project.Inputs.MainCharInput>();
        playerSounds = GetComponent<MainCharSounds>();
        playerHealth = GetComponent<Health>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        isInAir = false;
        isJumping = false;
        isDoubleJumping = false;
        canMove = true;
        numberOfJumps = 0;
        defaultColliderSize = playerCollider.size;
    }

    //В Update работают методы, проверяющие в воздухе ли игрок и жив ли он.
    private void Update()
    {
        CheckIsInAir();
        isAlive();
    }

    // В Fixed Update перемещаем персонажа, также выполняем прыжки и двойные прыжки.
    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveCharacter();
            JumpAndDoubleJumpCharacter();
        }
    }

    /* В методе OnValidate проверяем не установлена ли в редакторе
     * скорость, а также сила прыжка меньше нуля. Если да, присваиваем ноль.
     */
    private void OnValidate()
    {
        if (movementSpeed < 0)
        {
            movementSpeed = 0;
        }
        else if (jumpForce < 0)
        {
            jumpForce = 0;
        }
    }

    /* Метод кастует капсуль, который равен триггеру на персонаже.
     * Если игрок находится на земле или на враге, то он не воздухе.
     * Если же нет, тогда он в воздухе.
     */
    private void CheckIsInAir()
    {
        if (Physics2D.CapsuleCast(groundCheckCapsule.transform.position, 
                                  groundCheckCapsule.size, groundCheckCapsule.direction, 
                                  groundCheckCapsule.transform.rotation.z, Vector2.zero, 0f, groundMask))
        {
            isInAir = false;
        }
        else if (Physics2D.CapsuleCast(groundCheckCapsule.transform.position, 
                                       groundCheckCapsule.size, groundCheckCapsule.direction, 
                                       groundCheckCapsule.transform.rotation.z, Vector2.zero, 0f, enemyMask))
        {
            isInAir = false;
        }
        else
        {
            isInAir = true;
        }
    }

    /* Метод перемещает персонажа, в зависимости от направления инпута клавиш A и D.
     * Если же мы отпускаем клавишу и игрок не в воздухе, он останавливается.
     */
    private void MoveCharacter()
    {
        if (Mathf.Abs(playerInput.HorizontalDirection) < 0.1 && Mathf.Abs(playerInput.HorizontalDirection) > 0  && isInAir == false)
        {
            playerRB.velocity = Vector2.zero;
        }
        else if (playerInput.HorizontalDirection > 0 && isInAir == false)
        {
            playerRB.velocity = Vector2.right * movementSpeed * Time.deltaTime;
        }
        else if (playerInput.HorizontalDirection < 0 && isInAir == false)
        {
            playerRB.velocity = Vector2.left * movementSpeed * Time.deltaTime;
        }
    }

    /* Метод осуществляет прыжок и двойной прыжок персонажа. Разделение в проверках основано на количестве прыжков (1 или 2),
     * в воздухе ли игрок и нажата ли кнопка прыжка. В зависимости от этого, у нас срабатывает первый или второй прыжок.
     */
    private void JumpAndDoubleJumpCharacter()
    {
        if (playerInput.JumpButtonPressed && isInAir == false && numberOfJumps == 0)
        {
            jumpPosition = 0f;
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            isJumping = true;
            playerSounds.PlayJumpStartSound();
            numberOfJumps++;
            playerInput.JumpButtonPressed = false;
        }
        else if (playerInput.JumpButtonPressed && isInAir && numberOfJumps == 0)
        {
            doubleJumpPosition = 0f;
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce * doubleJumpModifier);
            playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y - colliderDifference);
            isDoubleJumping = true;
            playerSounds.PlayJumpStartSound();
            numberOfJumps += 2;
            playerInput.JumpButtonPressed = false;
        }
        else if (playerInput.JumpButtonPressed && isInAir && numberOfJumps == 1)
        {
            doubleJumpPosition = 0f;
            isDoubleJumping = true;
            playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y - colliderDifference);
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce * doubleJumpModifier);
            playerSounds.PlayJumpStartSound();
            numberOfJumps++;
            playerInput.JumpButtonPressed = false;
        }
        else if (isInAir && numberOfJumps == 0)
        {
            isJumping = true;
            jumpPosition = 0.5f;
        }
        else if (isInAir == false)
        {
            playerCollider.size = defaultColliderSize;
            numberOfJumps = 0;
            doubleJumpPosition = 1f;
            isDoubleJumping = false;
            jumpPosition = 1f;
            isJumping = false;
        }
    }

    //Метод проверяет жив ли игрок. Если игрок умер - переключаем, что он не может двигаться
    private void isAlive()
    {
        if (playerHealth.IsAlive == false)
        {
            canMove = false;
        }
    }
    #endregion
}
