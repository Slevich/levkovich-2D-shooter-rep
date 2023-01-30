using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.GameSettings
{
    public class HUDController : MonoBehaviour
    {
        #region Поля
        [Header("Main character game object 'Joy'.")]
        [SerializeField] private GameObject mainChar;
        [Header("HUD, which shown on gameplay.")]
        [SerializeField] private GameObject levelPlayableHUD;
        [Header("Game object which contains screen on game pause.")]
        [SerializeField] private GameObject pauseScreen;
        [Header("Panel with button on pause screen.")]
        [SerializeField] private GameObject buttonsPanel;
        [Header("Buttons of control music and sounds. ")]
        [SerializeField] private GameObject musicOffButton;
        [SerializeField] private GameObject musicOnButton;
        [SerializeField] private GameObject soundsOffButton;
        [SerializeField] private GameObject soundsOnButton;
        [Header("Slider with current position of sounds and music.")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;
        [Header("Audio source with UI click sound.")]
        [SerializeField] private AudioSource UISoundsSource;
        [Header("All sound sources on level.")]
        [SerializeField] private AudioSource[] soundsSources;
        [Header("All music sources on level.")]
        [SerializeField] private AudioSource[] levelMusicSources;
        [Header("Arrays with max values of sound and music sources.")]
        [SerializeField] private float[] soundsMaxVolume;
        [SerializeField] private float[] musicMaxVolume;

        //Переменная, хранящая в себе референс с классом методов по управлению игровых настроек.
        private GameSettingsMethods gameSettingsMethods;
        //Текущие экран и панель.
        private GameObject currentScreen;
        private GameObject currentPanel;
        //Текущее положение слайдеров музыки и звуков.
        private float currentMusicSliderValue;
        private float currentSoundsSliderValue;
        #endregion

        #region Методы

        /// <summary>
        /// На старте, получаем компонет в классом методов по управлению настройками.
        /// Загружаем настройки.
        /// Передаем в значение слайдеров те, которые сохранены в глобальном классе.
        /// В текущий экран передаем HUD игрока.
        /// Текущая панель - панель с кнопками.
        /// Получаем максимальные значения источников звука и музыки.
        /// После чего обновляем уровень звуков и музыка в соответствии с настройками.
        /// </summary>
        private void Start()
        {
            gameSettingsMethods = GetComponent<GameSettingsMethods>();
            gameSettingsMethods.LoadSettings();
            musicSlider.value = GlobalSettings.musicSliderPosition;
            soundsSlider.value = GlobalSettings.soundsSliderPosition;
            currentScreen = levelPlayableHUD;
            currentPanel = buttonsPanel;
            UpdateMaxVolumes();
            UpdateAudioSourcesVolumes();
        }

        /// <summary>
        /// При закрытии приложения, передаем текущие значения слайдеров в глобальные значения.
        /// Сохраняем настройки.
        /// </summary>
        private void OnApplicationQuit()
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
        }

        /// <summary>
        /// В Update активируем экран паузы по нажатии кнопки.
        /// Если экран активен - ставим игру на паузу.
        /// При поднятии кнопки мыши, обновляем значения источников звука.
        /// Отключаем компоненты игрока, чтобы не совершалось лишних действий.
        /// </summary>
        private void Update()
        {
            ActivatePauseScreenOnButton();

            if (pauseScreen.activeInHierarchy)
            {
                Time.timeScale = 0;
                mainChar.GetComponent<Inputs.MainCharInput>().FireButtonPressed = false;
                mainChar.GetComponent<MainCharWeapons>().canAim = false;
                mainChar.GetComponent<MainCharMovement>().canMove = false;

                if (Input.GetMouseButtonUp(0))
                {
                    UpdateAudioSourcesVolumes();
                }

                UpdateButtons();
            }
            else
            {
                mainChar.GetComponent<MainCharWeapons>().canAim = true;
                mainChar.GetComponent<MainCharMovement>().canMove = true;
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// Перезапуск текущей сцены.
        /// </summary>
        public void OnClickRetry()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Запуск сцены главного меню.
        /// </summary>
        public void OnClickMainMenu()
        {
            SceneManager.LoadSceneAsync(0);
        }

        /// <summary>
        /// Запуск следующей сцены. Если индекс следующей сцены больше последнего,
        /// то запускается сцена главного меню.
        /// </summary>
        public void OnClickNext()
        {
            if (SceneManager.GetActiveScene().buildIndex < 5)
            {
                SceneManager.LoadSceneAsync((SceneManager.GetActiveScene().buildIndex) + 1);
            }
            else
            {
                SceneManager.LoadSceneAsync(0);
            }
        }

       /// <summary>
       /// Меняет текущий экран на другой.
       /// Если текущий экран - экран паузы, то все источники музыка
       /// ставятся на паузу. Если нет - проигрываются.
       /// </summary>
       /// <param name="nextScreen"></param>
        public void ChangeScreenOnClick(GameObject nextScreen)
        {
            currentScreen.SetActive(false);
            nextScreen.SetActive(true);
            currentScreen = nextScreen;

            if (currentScreen == pauseScreen)
            {
                for (int i = 0; i < levelMusicSources.Length; i++)
                {
                    if (levelMusicSources[i].isActiveAndEnabled)
                    {
                        levelMusicSources[i].Pause();
                    }
                }
            }
            else
            {
                for (int i = 0; i < levelMusicSources.Length; i++)
                {
                    if (levelMusicSources[i].isActiveAndEnabled)
                    {
                        levelMusicSources[i].Play();
                    }
                }
            }
        }

        /// <summary>
        /// При нажатии на кнопку Escape, если текущий экран не экран паузы,
        /// открывается он.
        /// Если текущий экран - экран паузы, то экран паузы скрывается.
        /// </summary>
        private void ActivatePauseScreenOnButton()
        {
            if (mainChar.GetComponent<Inputs.MainCharInput>().EscapeButtonPressed && currentScreen != pauseScreen)
            {
                currentScreen.SetActive(false);
                pauseScreen.SetActive(true);
                currentScreen = pauseScreen;
            }
            else if (mainChar.GetComponent<Inputs.MainCharInput>().EscapeButtonPressed && currentScreen == pauseScreen)
            {
                currentScreen.SetActive(false);
                levelPlayableHUD.SetActive(true);
                currentScreen = levelPlayableHUD;
            }
        }

        /// <summary>
        /// Меняет текущую панель.
        /// </summary>
        /// <param name="nextPanel"></param>
        public void ChangePanelOnClick(GameObject nextPanel)
        {
            currentPanel.SetActive(false);
            nextPanel.SetActive(true);
            currentPanel = nextPanel;
        }

        /// <summary>
        /// Проигрывает звук клика в меню.
        /// </summary>
        public void PlayClickSound()
        {
            UISoundsSource.Play();
        }

        /// <summary>
        /// Получаем максимальную громкость источников звука и
        /// музыки в цикле.
        /// </summary>
        private void UpdateMaxVolumes()
        {
            for (int a = 0; a < levelMusicSources.Length; a++)
            {
                musicMaxVolume[a] = levelMusicSources[a].volume;
            }

            for (int i = 0; i < soundsSources.Length; i++)
            {
                if (i == 0)
                {
                    soundsMaxVolume[i] = UISoundsSource.volume;
                }
                else
                {
                    soundsMaxVolume[i] = soundsSources[i].volume;
                }
            }
        }

        /// <summary>
        /// Метод отключает источник музыки, переводит слайдер в 0.
        /// Переключает кнопку.
        /// </summary>
        public void MusicOff()
        {
            currentMusicSliderValue = musicSlider.value;
            musicSlider.value = 0;
            musicOffButton.SetActive(false);
            musicOnButton.SetActive(true);
        }

        /// <summary>
        /// Метод включает источник музыки, переводит слайдер в 0.
        /// Переключает кнопку.
        /// </summary>
        public void MusicOn()
        {
            if (currentMusicSliderValue == 0) musicSlider.value = 0.5f;
            else musicSlider.value = currentMusicSliderValue;
            musicOffButton.SetActive(true);
            musicOnButton.SetActive(false);
        }

        /// <summary>
        /// Метод отключает источник звука, переводит слайдер в 0.
        /// Переключает кнопку.
        /// </summary>
        public void SoundsOff()
        {
            currentSoundsSliderValue = soundsSlider.value;
            soundsSlider.value = 0;
            soundsOffButton.SetActive(false);
            soundsOnButton.SetActive(true);
        }

        /// <summary>
        /// Метод включает источник звука, переводит слайдер в 0.
        /// Переключает кнопку.
        /// </summary>
        public void SoundsOn()
        {
            soundsSlider.value = currentSoundsSliderValue;
            soundsOffButton.SetActive(true);
            soundsOnButton.SetActive(false);
        }

        /// <summary>
        /// Метод устанавливает в циклах громкость источников звука,
        /// в соответствии с положениями слайдеров, опираясь на их
        /// максимальную громкость.
        /// </summary>
        private void UpdateAudioSourcesVolumes()
        {
            for (int i = 0; i < levelMusicSources.Length; i++)
            {
                levelMusicSources[i].volume = (musicSlider.value * musicMaxVolume[i]);
            }

            for (int i = 0; i < soundsSources.Length; i++)
            {
                if (i == 0)
                {
                    UISoundsSource.volume = (soundsSlider.value * soundsMaxVolume[i]);
                }
                else
                {
                    soundsSources[i].volume = (soundsSlider.value * soundsMaxVolume[i]);
                }
            }
        }

        /// <summary>
        /// Метод переключает кнопки включения/выключения звуков/музыки
        /// в соответствии с положением слайдеров.
        /// </summary>
        private void UpdateButtons()
        {
            if (musicSlider.value == 0)
            {
                musicOffButton.SetActive(false);
                musicOnButton.SetActive(true);
            }
            else
            {
                musicOnButton.SetActive(false);
                musicOffButton.SetActive(true);
            }

            if (soundsSlider.value == 0)
            {
                soundsOffButton.SetActive(false);
                soundsOnButton.SetActive(true);
            }
            else
            {
                soundsOnButton.SetActive(false);
                soundsOffButton.SetActive(true);
            }
        }
        #endregion
    }
}
