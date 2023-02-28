using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Project.GameSettings
{
    public abstract class BaseMenuManager : MonoBehaviour
    {
        #region Поля
        [Header("Panel with buttons in menu.")]
        [SerializeField] protected GameObject menuButtonsPanel;
        [Header("Button which manipulates music and sounds.")]
        [SerializeField] protected GameObject musicOffButton;
        [SerializeField] protected GameObject musicOnButton;
        [SerializeField] protected GameObject soundsOffButton;
        [SerializeField] protected GameObject soundsOnButton;
        [Header("Sliders, with current position of sounds and music.")]
        [SerializeField] protected Slider musicSlider;
        [SerializeField] protected Slider soundsSlider;

        //Переменная, хранящая в себе референс с классом методов по управлению игровых настроек.
        protected GameSettingsMethods gameSettingsMethods;
        //Текущая панель.
        protected GameObject currentPanel;
        //Текущее положение слайдеров музыки и звуков.
        protected float currentMusicSliderValue;
        protected float currentSoundsSliderValue;
        #endregion

        #region Методы
        //Метод получает нужные компоненты.
        protected void GetNecessaryComponents() => gameSettingsMethods = GetComponent<GameSettingsMethods>();
        /// <summary>
        /// Метод загружает сцену по индексу.
        /// </summary>
        /// <param name="SceneIndex">Индекс сцены в билде.</param>
        public abstract void LoadSceneOnClick(int SceneIndex);
        /// <summary>
        /// При смене панели, передаем значения слайдеров в глобальные переменны.
        /// Сохраняем настройки. Меняем панель.
        /// </summary>
        /// <param name="nextPanel">Следующая активируемая панель.</param>
        public abstract void ChangePanel(GameObject nextPanel);
        /// <summary>
        /// Метод проигрывает звук по клику.
        /// </summary>
        /// <param name="ClickSound">Аудио клип со звуком при клике.</param>
        public abstract void PlaySoundOnClick(AudioClip ClickSound);
        #endregion
    }
}
