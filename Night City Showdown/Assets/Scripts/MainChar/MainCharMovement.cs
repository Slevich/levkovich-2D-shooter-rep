using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharMovement : MonoBehaviour
{
    #region Поля
    [Range(1, 100)]
    [Header("Value of movement speed of character.")]
    [SerializeField] private float movementSpeed;
    [Range(0.1f, 10)]
    [Header("Value of jump force modifier.")]
    [SerializeField] private float jumpForce;
    [Header("Value of double jump force modifier.")]
    [SerializeField] private float doubleJumpModifier;

    //Переменная, обозначающая прыгает ли игрок.
    private bool isJumping;
    //Переменная, обозначающая в двойном ли прыжке игрок.
    private bool isDoubleJumping;
    //Переменная, обозначающая, может ли игрок двигаться.
    private bool canMove;
    //Переменная, обозначающая в какой позиции сейчас игрок в прыжке (старт, в воздухе, конец).
    private float jumpPosition;
    //Переменная, обозначающая в какой позиции сейчас игрок в двойном прыжке (старт, в воздухе, конец).
    private float doubleJumpPosition;
    //Переменная, содержащая референс на компонент с пользовательским инпутом.
    private Project.Inputs.MainCharInput playerInput;
    //Переменная, содержащая референс на компонент со звуками персонажа.
    private MainCharSounds playerSounds;
    ////Переменная, содержащая референс на компонент с проверками игрока.
    private MainCharChecks playerChecks;
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
    public bool IsJumping { get { return isJumping; } }
    public bool IsDoubleJumping { get { return isDoubleJumping; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public float JumpPosition { get { return jumpPosition; } }
    public float DoubleJumpPosition { get { return doubleJumpPosition; } }
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
        playerChecks = GetComponent<MainCharChecks>();
        playerHealth = GetComponent<Health>();
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        isJumping = false;
        isDoubleJumping = false;
        canMove = true;
        numberOfJumps = 0;
        defaultColliderSize = playerCollider.size;
    }

    //В Update работают методы, проверяющие в воздухе ли игрок и жив ли он.
    private void Update()
    {
        if (playerHealth.IsAlive == false) canMove = false;
    }

    // В Fixed Update перемещаем персонажа, также выполняем прыжки и двойные прыжки.
    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveCharacter();
            JumpControl(numberOfJumps);
        }
    }

    /* В методе OnValidate проверяем не установлена ли в редакторе
     * скорость, а также сила прыжка меньше минимального значения. Если да, присваиваем минимальное значение.
     */
    private void OnValidate()
    {
        if (movementSpeed < 1)
        {
            movementSpeed = 1;
        }
        else if (jumpForce < 0.1f)
        {
            jumpForce = 0.1f;
        }
    }

    /* Метод перемещает персонажа, в зависимости от направления инпута клавиш A и D.
     * Если же мы отпускаем клавишу и игрок не в воздухе, он останавливается.
     */
    private void MoveCharacter()
    {
        if (Mathf.Abs(playerInput.HorizontalDirection) < 0.1 && Mathf.Abs(playerInput.HorizontalDirection) > 0  && playerChecks.IsInAir == false)
        {
            playerRB.velocity = Vector2.zero;
        }
        else if (playerInput.HorizontalDirection > 0 && playerChecks.IsInAir == false)
        {
            playerRB.velocity = Vector2.right * movementSpeed * Time.deltaTime;
        }
        else if (playerInput.HorizontalDirection < 0 && playerChecks.IsInAir == false)
        {
            playerRB.velocity = Vector2.left * movementSpeed * Time.deltaTime;
        }
    }

    /* Метод, в зависимости от номера прыжка, вызывает те или иные методы.
     */
    private void JumpControl(int numberOfJumps)
    {
        switch(numberOfJumps)
        {
            //Стартовая позиция игрока.
            case 0:
                if (playerInput.JumpButtonPressed && playerChecks.IsInAir == false)
                {
                    Jump();
                }
                else if (playerInput.JumpButtonPressed == false && playerChecks.IsInAir)
                {
                    Fall();
                }
                else if (playerInput.JumpButtonPressed && playerChecks.IsInAir)
                {
                    DoubleJump(2);
                }
                else if (playerChecks.IsInAir == false)
                {
                    GroundCollision();
                }
            break;
            //Игрок уже прыгнул один раз. Дальше может только второй раз прыгнуть, падать и удариться об землю.
            case 1:
                if (playerInput.JumpButtonPressed && playerChecks.IsInAir)
                {
                    DoubleJump(1);
                }
                else if (playerInput.JumpButtonPressed == false && playerChecks.IsInAir)
                {
                    Fall();
                }
                else if (playerChecks.IsInAir == false)
                {
                    GroundCollision();
                }
                break;
            //Игрок либо прыгнул два раза, либо в воздухе прыгнул. Может только падать и удариться об землю.
            case 2:
                if (playerChecks.IsInAir)
                {
                    Fall();
                }
                else if (playerChecks.IsInAir == false)
                {
                    GroundCollision();
                }
            break;
        }
    }

    /* Метод в качестве позиции прыжка ставит начальную (отталкивается от земли).
     * Изменяет направление движения на движение вверх.
     * Переключает переменную прыжка в true, проигрывает звук прыжка.
     * Прибавляет к количестве прыжков единицу.
     * Отжимает кнопку прыжка.
     */ 
    private void Jump()
    {
        jumpPosition = 0f;
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        isJumping = true;
        playerSounds.PlayJumpStartSound();
        numberOfJumps++;
        playerInput.JumpButtonPressed = false;
    }

    /* Метод меняет позицию прыжка на стартовую.
     * Изменяет направление движения на движение вверх.
     * Изменяет размер коллайдера игрока на более меньший.
     * Переключает переменную двойного прыжка в true, проигрывает звук прыжка.
     * Прибавляет к количеству прыжков необходимое.
     * Отжимает кнопку прыжка.
     */
    private void DoubleJump(int JumpCost)
    {
        doubleJumpPosition = 0f;
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce * doubleJumpModifier);
        playerCollider.size = new Vector2(playerCollider.size.x, playerCollider.size.y - colliderDifference);
        isDoubleJumping = true;
        playerSounds.PlayJumpStartSound();
        numberOfJumps += JumpCost;
        playerInput.JumpButtonPressed = false;
    }

    /* Метод переключает переменную прыжка в true.
     * Меняет позицию прыжка на "в воздухе".
     */
    private void Fall()
    {
        isJumping = true;
        jumpPosition = 0.5f;
    }

    /* Метод возвращает размер колайдера игрока к дефолтному.
     * Обнуляет количество прыжков.
     * Ставит позиции прыжка и двойного прыжка в конечное (падение на землю).
     * Переключает переменные прыжка и двойного прыжка в false.
     */
    private void GroundCollision()
    {
        playerCollider.size = defaultColliderSize;
        numberOfJumps = 0;
        doubleJumpPosition = 1f;
        jumpPosition = 1f;
        isDoubleJumping = false;
        isJumping = false;
    }
    #endregion
}