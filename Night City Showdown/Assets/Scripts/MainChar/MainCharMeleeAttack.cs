using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharMeleeAttack : MainCharWeaponBase
{
    #region Поля
    [Header("Damage amount, which applies to enemies health, when character hit by fist.")]
    [SerializeField] private float hitDamage;
    [Header("Damage amount, which applies to enemies health, when character punch by foot")]
    [SerializeField] private float punchDamage;

    //Коллайдер врага.
    private Collider2D enemyCollider;
    //Переменная, обозначающая в радиусе атаки ли враг.
    private bool enemyInRange;
    //Переменная, обозначающая бьет ли игрок рукой.
    private bool isHitting;
    //Переменная, обозначающая пинает ли игрок ногой.
    private bool isPunching;
    #endregion

    #region Свойства
    //Свойства для получения и изменения полей.
    public Collider2D EnemyCollider { get { return enemyCollider; } set { enemyCollider = value; } }
    public bool EnemyInRange { get { return enemyInRange; } set { enemyInRange = value; } }
    public bool IsHitting { get { return isHitting; } }
    public bool IsPunching { get { return isPunching; } }
    #endregion

    #region Методы
    /*На старте, получаем необходимые компоненты.
     *Добавляем кулаки как оружие по умолчанию и делаем их активными.
     *При этом коллайдер врага делаем null.
     *Переводим переменные в false.
     */
    private void Start()
    {
        GetNecessaryComponents();
        AddWeaponToList("Fists");
        ChangeActiveWeapon("Fists");
        enemyCollider = null;
        isPunching = false;
        isHitting = false;
    }
    /*В апдейте проверяем, может ли атаковать игрок, не экипировано ли оружие и жив ли он.
     *Если да, мы может атаковать в мили врага.
     */
    private void Update()
    {
        if (canAttack && isWeaponEquipped == false && playerHealth.IsAlive)
        {
            HitSomeone();
        }
    }

    //Метод, переключает переменные, если нажата клавиша, что игрок бьет рукой или пинает.
    private void HitSomeone()
    {
        if (ActiveWeaponType == "Fists" && playerInput.FireButtonPressed && isPunching == false)
        {
            isHitting = true;
        }
        else if (ActiveWeaponType == "Fists" && playerInput.PunchButtonPressed && isHitting == false)
        {
            isPunching = true;
        }
    }

    /* Метод позволяет прекратить проигрывание анимации (она проигрывается один раз),
     * даже если кнопка удара или пинка продолжает быть нажатой.
     */
    public void UnHit()
    {
        isHitting = false;
        playerInput.FireButtonPressed = false;
        isPunching = false;
        playerInput.PunchButtonPressed = false;
    }

    /* Метод наносит урон противнику, находящемуся в зоне атаки ближнего боя,
     * а также толкает его в нужном направлении (только при пинке).
     */
    public void DamageInMelee()
    {
        if (isHitting && enemyInRange && enemyCollider != null)
        {
            enemyCollider.GetComponent<Health>().ToDamage(hitDamage);
            playerSounds.PlayHitSound();
        }
        else if (isPunching && enemyInRange && enemyCollider != null)
        {
            enemyCollider.GetComponent<Health>().ToDamage(punchDamage);
            playerSounds.PlayHitSound();
        }
    }
    #endregion
}
