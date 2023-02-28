using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Inputs
{
    public class MainCharInput : MonoBehaviour
    {
        #region Свойства
        public float HorizontalDirection { get; set; }
        public bool InputIsActive { get; set; }
        public bool JumpButtonPressed { get; set; }
        public bool Number1Pressed { get; set; }
        public bool Number2Pressed { get; set; }
        public bool Number3Pressed { get; set; }
        public bool FireButtonPressed { get; set; }
        public bool PunchButtonPressed { get; set; }
        public bool ReloadButtonPressed { get; set; }
        public bool EscapeButtonPressed { get; set; }

        //Переменная, хранящая референс на компонент, управляющий оружием игрока.
        private MainCharRangeAttack playerWeapons;
        #endregion

        #region Методы
        // На старте получаем компонент.
        private void Start()
        {
            playerWeapons = GetComponent<MainCharRangeAttack>();
            InputIsActive = true;
        }

        // В Update вызываем методы по отслеживанию нажатий.
        private void Update()
        {
            if (InputIsActive)
            {
                HorizontalDirection = Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);
                IsJumpButtonPressed();
                IsNumberButtonsPressed();
                IsLeftMouseButtonPressed();
                IsRightMouseButtonPressed();
                IsReloadButtonPressed();
                IsEscapeButtonPressed();
            }
        }

        // Метод отслеживает и передает в переменную, нажата ли клавиша прыжка (пробел).
        private void IsJumpButtonPressed()
        {
            if (Input.GetButtonDown(GlobalStringVars.JUMP_BUTTON))
            {
                JumpButtonPressed = true;
            }
        }

        // Метод отслеживает и передает в переменную, нажати ли клавиши 1,2,3.
        private void IsNumberButtonsPressed()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Number1Pressed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Number2Pressed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Number3Pressed = true;
            }
        }

        // Метод отслеживает и передает в переменную, нажата или отжата левая кнопка мыши.
        private void IsLeftMouseButtonPressed()
        {
            if (Input.GetButtonDown(GlobalStringVars.FIRE_BUTTON))
            {
                FireButtonPressed = true;
            }
            else if (Input.GetButtonUp(GlobalStringVars.FIRE_BUTTON))
            {
                FireButtonPressed = false;
            }
        }

        // Метод отслеживает и передает в переменную, нажата или отжата правая кнопка мыши.
        private void IsRightMouseButtonPressed()
        {
            if (Input.GetButtonDown(GlobalStringVars.PUNCH_BUTTON))
            {
                PunchButtonPressed = true;
            }
            else if (Input.GetButtonUp(GlobalStringVars.PUNCH_BUTTON))
            {
                PunchButtonPressed = false;
            }
        }

        /* Метод отслеживает и передает в переменную, 
         * нажата ли клавиша перезарядки (R) и говорит, 
         * что перезарядка начата.
         */
        private void IsReloadButtonPressed()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerWeapons.IsReloading = true;
                ReloadButtonPressed = true;
            }
        }

        // Метод отслеживает и передает в переменную, нажата ли клавиша Escape.
        private void IsEscapeButtonPressed()
        {
            if (Input.GetButtonDown(GlobalStringVars.ESCAPE_BUTTON))
            {
                EscapeButtonPressed = true;
            }
            else if (Input.GetButtonUp(GlobalStringVars.ESCAPE_BUTTON))
            {
                EscapeButtonPressed = false;
            }
        }
        #endregion
    }
}
