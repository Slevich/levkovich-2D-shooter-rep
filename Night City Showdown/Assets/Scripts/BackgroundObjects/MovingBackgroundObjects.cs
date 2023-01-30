using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackgroundObjects : MonoBehaviour
{
    #region Переменные
    [Header("Movement speed of background objects")]
    [SerializeField] private float speed;
    [Header("Movement direction of background objects")]
    [SerializeField] private string direction;
    [Header("Distance outside the camera area for turning")]
    [SerializeField] private float rotationOffset;

    //Rigidbody2D движущегося объекта.
    private Rigidbody2D truckRB;
    //Главная камера.
    private Camera mainCamera;
    //Максимальная и минимальная координаты по X для движения объекта.
    private float minXCoordinate;
    private float maxXCoordinate;
    #endregion

    #region Методы
    /// <summary>
    /// Метод Start. 
    /// На старте получаем Rigidbody2d, передаем главную камеру в переменную.
    ///Если направление стоит "backward" объекта на старте разворачивается.
    /// </summary>
    void Start()
    {
        truckRB = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        if (direction == "Backward") transform.Rotate(0, 180, 0);
    }

    /// <summary>
    /// Метод Update.
    /// В апдейте считываем максимальную и минимальную координаты по X.
    /// Далее движение объекта.
    /// </summary>
    void Update()
    {
        CalculateXCoodinateBorders();
        MoveTruck();
    }

    /// <summary>
    /// Рассчитываем минимальную и максимальную точку по X,
    /// Путем вычета или сложения размера камеры, расстояния движения объекта
    /// вне камеры и местоположения камеры.
    /// </summary>
    private void CalculateXCoodinateBorders()
    {
        minXCoordinate = mainCamera.transform.position.x - mainCamera.orthographicSize - rotationOffset;
        maxXCoordinate = mainCamera.transform.position.x + mainCamera.orthographicSize + rotationOffset;
    }

    /// <summary>
    /// Двигаем объект в записимости от направления движения.
    /// Доходя до крайней точки, объект разворачивается и
    /// движется в обратную сторону.
    /// </summary>
    private void MoveTruck()
    {
        if (direction == "Forward")
        {
            truckRB.velocity = Vector2.right * speed;

            if (transform.position.x > maxXCoordinate)
            {
                transform.Rotate(0, 180, 0);
                direction = "Backward";
            }
        }
        else if (direction == "Backward")
        {
            truckRB.velocity = Vector2.left * speed;

            if (transform.position.x < minXCoordinate)
            {
                transform.Rotate(0, -180, 0);
                direction = "Forward";
            }
        }
    }
    #endregion
}
