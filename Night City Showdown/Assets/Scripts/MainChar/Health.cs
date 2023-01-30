using UnityEngine;

public class Health : MonoBehaviour
{
    #region Поля
    [Header("Current health's amount of character.")]
    [SerializeField] protected float currentHealth;
    [Header("Maximal health's amount of character.")]
    [SerializeField] protected float maxHealth;
    #endregion

    #region Свойства
    public bool IsAlive { get { return currentHealth > 0; } }

    #endregion

    #region Методы
    /* На старте, значение, что первонаж жив - истинно.
     * Текущее здоровье становится равным максимальному.
     */
    private void Start()
    {
        currentHealth = maxHealth;
    }

    /* При измении параметров в инспекторе, отслеживаем,
     * чтобы текущее здоровье не было больше максимального, или меньше нуля.
     * Также отслеживаем, чтобы максимальное не было меньше текущего или нуля.
     */
    private void OnValidate()
    {
        if (currentHealth < 0) currentHealth = 0;
        else if (currentHealth > maxHealth) currentHealth = maxHealth;
        else if (maxHealth < currentHealth) maxHealth = currentHealth;
        else if (maxHealth < 0) maxHealth = currentHealth;
    }

    /// <summary>
    /// Метод прибавляет к текущему здоровью определенное значение.
    /// Если при прибавлении текущее здоровье больше максимального - прибавляется их разница.
    /// Если нет, прибавляется указанное в параметре значение.
    /// </summary>
    /// <param name="healAmount">Количество восстанавливаемого здоровья.</param>
    public void ToHeal(float healAmount)
    {
        if ((currentHealth + healAmount) > maxHealth) currentHealth += (maxHealth - currentHealth);
        else currentHealth += healAmount;
    }

    /// <summary>
    /// Метод вычисляет текущий процент здоровья.
    /// </summary>
    /// <returns>Соотношение текущего и максимального здоровья.</returns>
    public float GetCurrentHealthProcent()
    {
        float healthProcent = currentHealth / maxHealth;
        return healthProcent;
    }

    /// <summary>
    /// Метод наносит текущему здоровью урон
    /// в установленном размере.
    /// </summary>
    /// <param name="damage">Количество наносимого уровна.</param>
    public virtual void ToDamage(float damage)
    {
        currentHealth -= damage;
    }
    #endregion
}
