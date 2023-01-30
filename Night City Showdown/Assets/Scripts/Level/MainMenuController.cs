using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.GameSettings
{
    public class MainMenuController : MonoBehaviour
    {
        #region Переменные
        [Header("Panel with buttons in main menu.")]
        [SerializeField] private GameObject mainMenuButtonsPanel;
        [Header("Audio source with click sound of buttons.")]
        [SerializeField] private AudioSource buttonsSoundsAudioSource;
        [Header("Audio source with main menu music.")]
        [SerializeField] private AudioSource mainMenuMusicSource;
        [Header("Button which manipulates music and sounds.")]
        [SerializeField] private GameObject musicOffButton;
        [SerializeField] private GameObject musicOnButton;
        [SerializeField] private GameObject soundsOffButton;
        [SerializeField] private GameObject soundsOnButton;
        [Header("Sliders, with current position of sounds and music.")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;

        //Переменная, хранящая в себе референс с классом методов по управлению игровых настроек.
        private GameSettingsMethods gameSettingsMethods;
        //Текущая панель.
        private GameObject currentPanel;
        //Максимальны значения громкости музыки и звуков.
        private float musicMaxVolume;
        private float soundsMaxVolume;
        //Текущее значение громкости музыки и звуков.
        private float currentMusicVolume;
        private float currentSoundsVolume;
        #endregion

        #region Методы
        /// <summary>
        /// На старте, передаем все необходимые параметры в переменные.
        /// Загружаем настройки слайдеров.
        /// Передаем в значения слайдеров значения из глобальных переменных настроек.
        /// Обновляем значения источников звука и музыки в соответствии со значением
        /// слайдеров и максимальной громкостью.
        /// </summary>
        void Start()
        {
            gameSettingsMethods = GetComponent<GameSettingsMethods>();
            currentPanel = mainMenuButtonsPanel;
            musicMaxVolume = mainMenuMusicSource.volume;
            soundsMaxVolume = buttonsSoundsAudioSource.volume;
            currentMusicVolume = 0.5f;
            currentSoundsVolume = 0.5f;
            gameSettingsMethods.LoadSettings();
            musicSlider.value = GlobalSettings.musicSliderPosition;
            soundsSlider.value = GlobalSettings.soundsSliderPosition;
            mainMenuMusicSource.volume = (musicSlider.value * musicMaxVolume);
            buttonsSoundsAudioSource.volume = (soundsSlider.value * soundsMaxVolume);
        }

        /// <summary>
        /// При выходе из приложения, передаем положения слайдеров в глобальные переменные.
        /// После чего сохраняем текущие настройки.
        /// </summary>
        private void OnApplicationQuit()
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
        }

        /// <summary>
        /// В Update обновляем значения источников звуки и музыки в соответствии
        /// с положениями слайдеров.
        /// Если значения слайдера равно нулю, переключаем кнопки.
        /// </summary>
        void Update()
        {
            mainMenuMusicSource.volume = (musicSlider.value * musicMaxVolume);
            buttonsSoundsAudioSource.volume = (soundsSlider.value * soundsMaxVolume);


            if (musicSlider.value == 0)
            {
                musicOffButton.SetActive(false);
                musicOnButton.SetActive(true);
            }
            else
            {
                musicOffButton.SetActive(true);
                musicOnButton.SetActive(false);
            }

            if (soundsSlider.value == 0)
            {
                soundsOffButton.SetActive(false);
                soundsOnButton.SetActive(true);
            }
            else
            {
                soundsOffButton.SetActive(true);
                soundsOnButton.SetActive(false);
            }
        }

        /// <summary>
        /// При смене панели, передаем значения слайдеров в глобальные переменны.
        /// Сохраняем настройки.
        /// Меняем панель.
        /// </summary>
        /// <param name="nextPanel"></param>
        public void ChangePanel(GameObject nextPanel)
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
            currentPanel.SetActive(false);
            nextPanel.SetActive(true);
            currentPanel = nextPanel;
        }

        /// <summary>
        /// При активации панели, передаем значения слайдеров в глобальные переменны,
        /// сохраняем настройки.
        /// Активируем панель.
        /// </summary>
        /// <param name="panel"></param>
        public void ActivatePanel(GameObject panel)
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
            panel.SetActive(true);
        }

        /// <summary>
        /// При дизаактивации панели, передаем значения слайдеров в глобальные переменны,
        /// сохраняем настройки.
        /// Активируем панель.
        /// </summary>
        /// <param name="panel"></param>
        public void DisactivatePanel(GameObject panel)
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
            panel.SetActive(false);
        }

        /// <summary>
        /// Проигрывает звук клика.
        /// </summary>
        /// <param name="soundClip"></param>
        public void PlaySoundOnClick(AudioClip soundClip)
        {
            buttonsSoundsAudioSource.clip = soundClip;
            buttonsSoundsAudioSource.Play();
        }

        /// <summary>
        /// Метод выключает музыку. При этом передаем бывшую громкость источника музыка.
        /// Переключаем слайдер в нулевое значение.
        /// Меняем кнопки.
        /// </summary>
        public void MusicOff()
        {
            currentMusicVolume = mainMenuMusicSource.volume;
            musicSlider.value = 0;
            musicOffButton.SetActive(false);
            musicOnButton.SetActive(true);
        }

        /// <summary>
        /// Метод включает музыку. Возвращаем значение слайдеру то, которое было до отключения.
        /// Переключаем слайдер в нулевое значение.
        /// Меняем кнопки.
        /// </summary>
        public void MusicOn()
        {
            musicSlider.value = (currentMusicVolume / musicMaxVolume);
            musicOffButton.SetActive(true);
            musicOnButton.SetActive(false);
        }

        /// <summary>
        /// Метод выключает звуки. При этом передаем бывшую громкость источника музыка.
        /// Переключаем слайдер в нулевое значение.
        /// Меняем кнопки.
        /// </summary>
        public void SoundsOff()
        {
            currentSoundsVolume = buttonsSoundsAudioSource.volume;
            soundsSlider.value = 0;
            soundsOffButton.SetActive(false);
            soundsOnButton.SetActive(true);
        }

        /// <summary>
        /// Метод включает звуки. Возвращаем значение слайдеру то, которое было до отключения.
        /// Переключаем слайдер в нулевое значение.
        /// Меняем кнопки.
        /// </summary>
        public void SoundsOn()
        {
            soundsSlider.value = (currentSoundsVolume / soundsMaxVolume);
            soundsOffButton.SetActive(true);
            soundsOnButton.SetActive(false);
        }

        /// <summary>
        /// Загружает выбранную сцену в определенным индексом.
        /// Перед этим сохраняем положения слайдеров в глобальные переменные
        /// и сохраняем настройки.
        /// </summary>
        /// <param name="sceneNumber"></param>
        public void LoadScene(int sceneNumber)
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
            SceneManager.LoadScene(sceneNumber);
        }

        /// <summary>
        /// Метод закрывает приложение.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
        #endregion
    }
}
