using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSounds : MonoBehaviour
{
    #region Поля
    [Header("Array of footsteps sounds.")]
    [SerializeField] private AudioClip[] footstepsSounds;

    private AudioSource movementAudioSource;
    //Переменная, хранящая номер аудио клипа.
    private int clipNumber;
    #endregion

    #region Методы
    //Получаем у дочернего объекта компонент AudioSource.
    private void Awake()
    {
        movementAudioSource = GetComponentInChildren<AudioSource>();
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
