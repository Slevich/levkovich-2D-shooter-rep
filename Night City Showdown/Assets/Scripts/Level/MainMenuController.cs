using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.GameSettings
{
    public class MainMenuController : BaseMenuManager
    {
        #region Поля
        [Header("Panel with buttons in main menu.")]
        [SerializeField] private GameObject mainMenuButtonsPanel;
        [Header("Audio source with click sound of buttons.")]
        [SerializeField] private AudioSource buttonsSoundsAudioSource;
        [Header("Audio source with main menu music.")]
        [SerializeField] private AudioSource mainMenuMusicSource;

        //Максимальные значения громкости музыки и звуков.
        private float musicMaxVolume;
        private float soundsMaxVolume;
        #endregion

        #region Методы
        /* На старте, передаем все необходимые параметры в переменные.
        * Загружаем настройки слайдеров.
        * Передаем в значения слайдеров значения из глобальных переменных настроек.
        * Обновляем значения источников звука и музыки в соответствии со значением
        * слайдеров и максимальной громкостью.
        */
        private void Start()
        {
            GetNecessaryComponents();
            currentPanel = mainMenuButtonsPanel;
            musicMaxVolume = mainMenuMusicSource.volume;
            soundsMaxVolume = buttonsSoundsAudioSource.volume;
            currentMusicSliderValue = 0.5f;
            currentSoundsSliderValue = 0.5f;
            gameSettingsMethods.LoadSettings();
            musicSlider.value = GlobalSettings.musicSliderPosition;
            soundsSlider.value = GlobalSettings.soundsSliderPosition;
            mainMenuMusicSource.volume = (musicSlider.value * musicMaxVolume);
            buttonsSoundsAudioSource.volume = (soundsSlider.value * soundsMaxVolume);
        }

        /* При выходе из приложения, передаем положения слайдеров в глобальные переменные.
         * После чего сохраняем текущие настройки.
         */
        private void OnApplicationQuit()
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
        }

        /* В Update обновляем значения источников звуки и музыки в соответствии
         * с положениями слайдеров. Если значения слайдера равно нулю, переключаем кнопки.
         */
        private void Update()
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
        /// Сохраняем настройки. Меняем панель.
        /// </summary>
        /// <param name="nextPanel">Следующая активируемая панель.</param>
        public override void ChangePanel(GameObject nextPanel)
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
        /// сохраняем настройки. Активируем панель.
        /// </summary>
        /// <param name="panel">Активируемая панель.</param>
        public void ActivatePanel(GameObject panel)
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
            panel.SetActive(true);
        }

        /// <summary>
        /// При дизактивации панели, передаем значения слайдеров в глобальные переменны,
        /// сохраняем настройки. Активируем панель.
        /// </summary>
        /// <param name="panel">Дизактивируемая панель.</param>
        public void DisactivatePanel(GameObject panel)
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
            panel.SetActive(false);
        }

        /// <summary>
        /// Метод сохраняет настройки и загружает сцену по индексу.
        /// </summary>
        /// <param name="SceneIndex">Индекс загружаемой сцены.</param>
        public override void LoadSceneOnClick(int SceneIndex)
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
            if (SceneIndex < 5) SceneManager.LoadSceneAsync(SceneIndex);
            else SceneManager.LoadSceneAsync(0);
        }

        /// <summary>
        /// Проигрывает звук клика.
        /// </summary>
        /// <param name="soundClip">Audio Clip со звуком клика.</param>
        public override void PlaySoundOnClick(AudioClip soundClip)
        {
            buttonsSoundsAudioSource.clip = soundClip;
            buttonsSoundsAudioSource.Play();
        }

        /// <summary>
        /// Метод выключает музыку. При этом передаем бывшую громкость источника музыка.
        /// Переключаем слайдер в нулевое значение. Меняем кнопки.
        /// </summary>
        public void MusicOff()
        {
            currentMusicSliderValue = mainMenuMusicSource.volume;
            musicSlider.value = 0;
            musicOffButton.SetActive(false);
            musicOnButton.SetActive(true);
        }

        /// <summary>
        /// Метод включает музыку. Возвращаем значение слайдеру то, которое было до отключения.
        /// Переключаем слайдер в нулевое значение. Меняем кнопки.
        /// </summary>
        public void MusicOn()
        {
            musicSlider.value = (currentMusicSliderValue / musicMaxVolume);
            musicOffButton.SetActive(true);
            musicOnButton.SetActive(false);
        }

        /// <summary>
        /// Метод выключает звуки. При этом передаем бывшую громкость источника музыка.
        /// Переключаем слайдер в нулевое значение. Меняем кнопки.
        /// </summary>
        public void SoundsOff()
        {
            currentSoundsSliderValue = buttonsSoundsAudioSource.volume;
            soundsSlider.value = 0;
            soundsOffButton.SetActive(false);
            soundsOnButton.SetActive(true);
        }

        /// <summary>
        /// Метод включает звуки. Возвращаем значение слайдеру то, которое было до отключения.
        /// Переключаем слайдер в нулевое значение. Меняем кнопки.
        /// </summary>
        public void SoundsOn()
        {
            soundsSlider.value = (currentSoundsSliderValue / soundsMaxVolume);
            soundsOffButton.SetActive(true);
            soundsOnButton.SetActive(false);
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
