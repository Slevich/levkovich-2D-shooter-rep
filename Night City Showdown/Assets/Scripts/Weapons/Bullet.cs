using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Поля
    [Header("Prefab with 2D sprite animation on bullet destruction.")]
    [SerializeField] private GameObject bulletDestroyVFXPrefab;
    [Header("Distance of bullet life.")]
    [SerializeField] private float bulletFlyRange;
    [Header("Number of damage, on which player's health decreasing.")]
    [SerializeField] private float damage;
    [Header("String name of bullet's owner: 'Player' or 'Enemy'.")]
    [SerializeField] private string bulletOwner;

    // Точка в пространстве, с которого пуля начинает своё движение.
    private Vector2 bulletStartPosition;
    #endregion

    #region Методы
    //На старте передаем начальную позицию пуля в переменную.
    private void Awake()
    {
        bulletStartPosition = transform.position;
    }

    /* Проверяем дистанцию, которую пролетает куля.
     * Если она больше значения, она уничтожается.
     */
    private void Update()
    {
        if (Vector2.Distance(bulletStartPosition, transform.position) >= bulletFlyRange)
        {
            Destroy(gameObject);
        }
    }

    /* При попадании в триггер пуля объектов с тегами
     * "Земля" или "Враг", спавнит префаб с эффектом и уничтожается.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GameObject bulletDestroyVFX = Instantiate(bulletDestroyVFXPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy") && bulletOwner == "Player")
        {
            GameObject bulletDestroyVFX = Instantiate(bulletDestroyVFXPrefab, transform.position, Quaternion.identity);
            collision.GetComponent<EnemyHealth>().ToDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player") && bulletOwner == "Enemy")
        {
            GameObject bulletDestroyVFX = Instantiate(bulletDestroyVFXPrefab, transform.position, Quaternion.identity);
            collision.GetComponent<Health>().ToDamage(damage);
            Destroy(gameObject);
        }
    }
    #endregion
}
