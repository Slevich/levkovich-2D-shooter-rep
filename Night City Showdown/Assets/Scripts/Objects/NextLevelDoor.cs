using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelDoor : MonoBehaviour
{
    #region Поля
    [Header("Game Object prefab, which contains destroy VFX.")]
    [SerializeField] private GameObject destroyVFX;
    [Header("Image which show current HP level.")]
    [SerializeField] private Image HPBar;

    //Переменная, содержащая референс на компонент здоровья.
    private Health doorsHealth;
    #endregion

    #region Методы
    //На старте получаем компонент здоровья.
    private void Start()
    {
        doorsHealth = GetComponent<Health>();
    }

    /* В Update обновляем уровень здоровья двери.
     * Если дверь "умерла", то спавним префаб с эффектом уничтожения.
     * После чего уничтожаем дверь.
     */
    private void Update()
    {
        UpdateDoorHPLevel();

        if (doorsHealth.IsAlive == false)
        {
            GameObject explosion = Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    /* Расчитываем уровень здоровья двери.
     * Передаем его в Image c уровнем здоровья.
     */
    private void UpdateDoorHPLevel()
    {
        float doorHP = doorsHealth.GetCurrentHealthProcent();
        HPBar.fillAmount = doorHP;
    }
    #endregion
}
