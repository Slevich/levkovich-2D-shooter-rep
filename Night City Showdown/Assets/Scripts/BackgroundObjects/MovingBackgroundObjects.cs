using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackgroundObjects : MonoBehaviour
{
    #region Поля
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
    /* На старте получаем Rigidbody2d, передаем главную камеру в переменную.
     * Если направление стоит "backward" объекта на старте разворачивается.
     */
    void Start()
    {
        truckRB = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        if (direction == "Backward") transform.Rotate(0, 180, 0);
    }

    /* В апдейте считываем максимальную и минимальную координаты по X.
     * Далее движение объекта.
     */
    void Update()
    {
        CalculateXCoodinateBorders();
        MoveTruck();
    }

    /* Рассчитываем минимальную и максимальную точку по X,
     * Путем вычета или сложения размера камеры, расстояния движения объекта
     * вне камеры и местоположения камеры.
     */
    private void CalculateXCoodinateBorders()
    {
        minXCoordinate = mainCamera.transform.position.x - mainCamera.orthographicSize - rotationOffset;
        maxXCoordinate = mainCamera.transform.position.x + mainCamera.orthographicSize + rotationOffset;
    }

    /* Двигаем объект в записимости от направления движения.
     * Доходя до крайней точки, объект разворачивается и
     * движется в обратную сторону.
     */
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
