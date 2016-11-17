using UnityEngine;
using Zenject;
using System;

namespace GG
{
    public class InputHandler : ITickable
    {
        private Settings _settings;

        public Action<int> OnMove;
        public Action OnJump;
        public Action OnStopJump;
        public Action OnAttack;
        public Action OnSwitch;

        public InputHandler(Settings settings)
        {
            _settings = settings;
        }

        public void Tick()
        {
            moveCommand();
            jumpCommand();
            attackCommand();
            switchWorldCommand();
        }

        private void moveCommand()
        {
            var axis = Input.GetAxisRaw(_settings.horizontalAxis);

            if (OnMove != null)
                OnMove((int)axis);
        }

        private void jumpCommand()
        {
            if (Input.GetButtonDown(_settings.jumpButton))
                if (OnJump != null)
                    OnJump();

            if (Input.GetButtonUp(_settings.jumpButton))
                if (OnStopJump != null)
                    OnStopJump();
        }

        private void attackCommand()
        {
            if (Input.GetButtonDown(_settings.attackButton))
            {
                if (OnAttack != null)
                    OnAttack();
            }
        }

        private void switchWorldCommand()
        {
            if (Input.GetButtonDown(_settings.worldSwitchButton))
                if (OnSwitch != null)
                    OnSwitch();
        }

        [System.Serializable]
        public class Settings
        {
            public string horizontalAxis;
            public string jumpButton;
            public string attackButton;
            public string worldSwitchButton;
        }
    }
}