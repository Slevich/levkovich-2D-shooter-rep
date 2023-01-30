using UnityEngine;

namespace Project.GameSettings
{
    public class GameSettingsMethods : MonoBehaviour
    {
        #region Методы
        /// <summary>
        /// Метод передает по ключу в класс пользовательских настроек
        /// значение переменных с позициями слайдеров, хранящихся в глобальном классе.
        /// Затем сохраняет их.
        /// </summary>
        public void SaveSettings()
        {
            PlayerPrefs.SetFloat("MusicSliderPosition", GlobalSettings.musicSliderPosition);
            PlayerPrefs.SetFloat("SoundsSliderPosition", GlobalSettings.soundsSliderPosition);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Метод передает в переменные с позициями слайдеров из глобального класса, значения
        /// по ключам. При этом, проверяется, имеется ли там соответствующий ключ.
        /// Если же нет, тогда задается дефолтное значение в единицу.
        /// </summary>
        public void LoadSettings()
        {
            if (PlayerPrefs.HasKey("MusicSliderPosition"))
            {
                GlobalSettings.musicSliderPosition = PlayerPrefs.GetFloat("MusicSliderPosition");
                GlobalSettings.soundsSliderPosition = PlayerPrefs.GetFloat("SoundsSliderPosition");
            }
            else
            {
                PlayerPrefs.SetFloat("MusicSliderPosition", 1f);
                PlayerPrefs.SetFloat("SoundsSliderPosition", 1f);
            }
        }
        #endregion
    }
}
