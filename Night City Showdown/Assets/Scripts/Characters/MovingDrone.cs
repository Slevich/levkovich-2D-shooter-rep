using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDrone : MonoBehaviour
{
    #region Переменные
    [Header("Movement speed")]
    [SerializeField] private float speed;
    [Header("Waiting timer before starting movement")]
    [SerializeField] private float waitingTimer;
    [Header("Movement direction")]
    [SerializeField] private string direction;
    [Header("Forward movement destination point")]
    [SerializeField] private Vector2 forwardPoint;
    [Header("Backward movement destination point")]
    [SerializeField] private Vector2 backwardPoint;
    [Header("Distance between object and target to stop")]
    [SerializeField] private float stoppingDistance;

    //Sprite renderer объекта.
    private SpriteRenderer charSR;
    //Rigidbody2D объекта.
    private Rigidbody2D charRB;
    //Animator объекта.
    private Animator charAnim;
    //Активная точка перемещения.
    private Vector2 activePoint;
    //Переменная для обнуления таймера ожидания.
    private float reloadWaitingTimer;
    //Переменная, обозначающая достижение объектом точки назначения.
    private bool charReachPosition;
    #endregion

    #region Методы
    /// <summary>
    /// Метод Start. Получаем необходимые компоненты.
    /// Присваиваем таймеру обнуления - таймер.
    /// Активной точке присваиваем местоположение объекта.
    /// </summary>
    private void Start()
    {
        charSR = GetComponent<SpriteRenderer>();
        charRB = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();
        reloadWaitingTimer = waitingTimer;
        activePoint = transform.position;
    }

    /// <summary>
    /// Метод Update. Вызываем метод для проверки дошел ли объект до точки.
    /// Вызываем метод поворота спрайта в зависимости от направления движения.
    /// Если объект дошел до точки, останавливаем его, включаем таймер,
    /// потом меняем направление движения.
    /// </summary>
    private void Update()
    {
        CharReachPoint();
        UpdateSpriteFlip();

        if (charReachPosition)
        {
            charAnim.SetFloat("Speed", 0);
            charRB.velocity = Vector2.zero;
            waitingTimer -= Time.deltaTime;

            if (waitingTimer <= 0 && direction == "Forward")
            {
                activePoint = backwardPoint;
                direction = "Backward";
                waitingTimer = reloadWaitingTimer;
            }
            else if (waitingTimer <= 0 && direction == "Backward")
            {
                activePoint = forwardPoint;
                direction = "Forward";
                waitingTimer = reloadWaitingTimer;
            }
        }
        else
        {
            charAnim.SetFloat("Speed", 0.5f);
            charRB.velocity = (activePoint - new Vector2(transform.position.x, transform.position.y)) * speed;
        }
    }

    /// <summary>
    /// Проверка расстояния между transform объекта и дистанцией остановки.
    /// </summary>
    private void CharReachPoint()
    {
        if (Vector2.Distance(transform.position, activePoint) <= stoppingDistance) charReachPosition = true;
        else charReachPosition = false;
    }

    /// <summary>
    /// Поворот спрайта в зависимости от направления движения объекта.
    /// </summary>
    private void UpdateSpriteFlip()
    {
        if (charRB.velocity.x > 0) charSR.flipX = false;
        else if (charRB.velocity.x < 0) charSR.flipX = true;
    }
    #endregion
}
