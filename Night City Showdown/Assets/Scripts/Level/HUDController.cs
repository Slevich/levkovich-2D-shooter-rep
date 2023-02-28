using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.GameSettings
{
    public class HUDController : BaseMenuManager
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
        [Header("Audio source with UI click sound.")]
        [SerializeField] private AudioSource UISoundsSource;
        [Header("All sound sources on level.")]
        [SerializeField] private AudioSource[] soundsSources;
        [Header("All music sources on level.")]
        [SerializeField] private AudioSource[] levelMusicSources;

        //Массивы, хранящие максимальную громкость источников звука и музыки.
        private List<float> soundsMaxVolume = new List<float>();
        private List<float> musicMaxVolume = new List<float>();
        //Текущие экран.
        private GameObject currentScreen;
        #endregion

        #region Методы
        /* На старте, получаем компонет в классом методов по управлению настройками.
         * Загружаем настройки.
         * Передаем в значение слайдеров те, которые сохранены в глобальном классе.
         * В текущий экран передаем HUD игрока.
         * Текущая панель - панель с кнопками.
         * Получаем максимальные значения источников звука и музыки.
         * После чего обновляем уровень звуков и музыка в соответствии с настройками.
         */
        private void Start()
        {
            GetNecessaryComponents();
            gameSettingsMethods.LoadSettings();
            musicSlider.value = GlobalSettings.musicSliderPosition;
            soundsSlider.value = GlobalSettings.soundsSliderPosition;
            currentScreen = levelPlayableHUD;
            currentPanel = buttonsPanel;
            UpdateMaxVolumes();
            UpdateAudioSourcesVolumes();
        }

        /* При закрытии приложения, передаем текущие значения слайдеров в глобальные значения.
         * Сохраняем настройки.
         */
        private void OnApplicationQuit()
        {
            GlobalSettings.musicSliderPosition = musicSlider.value;
            GlobalSettings.soundsSliderPosition = soundsSlider.value;
            gameSettingsMethods.SaveSettings();
        }

        /* В Update активируем экран паузы по нажатии кнопки.
         * Если экран активен - ставим игру на паузу.
         * При поднятии кнопки мыши, обновляем значения источников звука.
         * Отключаем компоненты игрока, чтобы не совершалось лишних действий.
         */
        private void Update()
        {
            if (pauseScreen.activeInHierarchy)
            {
                Time.timeScale = 0;
                mainChar.GetComponent<Inputs.MainCharInput>().FireButtonPressed = false;
                mainChar.GetComponent<MainCharRangeAttack>().CanAttack = false;

                if (Input.GetMouseButtonUp(0))
                {
                    UpdateAudioSourcesVolumes();
                }

                UpdateButtons();
            }
            else
            {
                mainChar.GetComponent<MainCharRangeAttack>().CanAttack = true;
                Time.timeScale = 1;
            }
        }

       /// <summary>
       /// Меняет текущий экран на другой.
       /// Если текущий экран - экран паузы, то все источники музыка
       /// ставятся на паузу. Если нет - проигрываются.
       /// </summary>
       /// <param name="nextScreen">Следующий активируемый экран.</param>
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
        /// Метод загружает сцену по индексу.
        /// </summary>
        /// <param name="SceneIndex">Индекс сцены в билде.</param>
        public override void LoadSceneOnClick(int SceneIndex)
        {
            if (SceneIndex < 5) SceneManager.LoadSceneAsync(SceneIndex);
            else SceneManager.LoadSceneAsync(0);
        }

        /// <summary>
        /// Метод загружает текущую сцену.
        /// </summary>
        public void ReloadCurrentSceneOnClick()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Метод загружает следующую сцену по индексу.
        /// </summary>
        public void LoadNextSceneOnClick()
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (sceneIndex < 5) SceneManager.LoadSceneAsync(sceneIndex + 1);
            else SceneManager.LoadSceneAsync(0);
        }

        /// <summary>
        /// Метод проигрывает звук клика.
        /// </summary>
        /// <param name="ClickSound">Аудио клип со звуком клика.</param>
        public override void PlaySoundOnClick(AudioClip ClickSound)
        {
            UISoundsSource.clip = ClickSound;
            UISoundsSource.Play();
        }

        //Получаем максимальную громкость источников звука и музыки в цикле.
        private void UpdateMaxVolumes()
        {
            for (int a = 0; a < levelMusicSources.Length; a++)
            {
                musicMaxVolume.Add(levelMusicSources[a].volume);
            }

            for (int i = 0; i < soundsSources.Length; i++)
            {
                if (i == 0)
                {
                    soundsMaxVolume.Add(UISoundsSource.volume);
                }
                else
                {
                    soundsMaxVolume.Add(soundsSources[i].volume);
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

        /* Метод устанавливает в циклах громкость источников звука,
         * в соответствии с положениями слайдеров, опираясь на их
         * максимальную громкость.
         */
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

        /* Метод переключает кнопки включения/выключения звуков/музыки
         * в соответствии с положением слайдеров.
         */
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
