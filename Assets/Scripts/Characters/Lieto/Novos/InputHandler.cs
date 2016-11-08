using UnityEngine;
using Zenject;
using System;

namespace GG
{
    public class InputEvents
    {
        public Action<int> OnMove { get; set; }
        public Action OnJump { get; set; }
        public Action OnStopJump { get; set; }
        public Action OnAttack { get; set; }
        public Action OnSwitch { get; set; }
    }

    public class InputHandler : ITickable
    {
        private Settings _settings;
        private InputEvents _inputEvents;

        public InputHandler(Settings settings, InputEvents _events)
        {
            _settings = settings;
            _inputEvents = _events;
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

            _inputEvents.OnMove((int)axis);
        }

        private void jumpCommand()
        {
            if (Input.GetButtonDown(_settings.jumpButton))
                _inputEvents.OnJump();

            if (Input.GetButtonUp(_settings.jumpButton))
                _inputEvents.OnStopJump();
        }

        private void attackCommand()
        {
            if (Input.GetButtonDown(_settings.attackButton))
                _inputEvents.OnAttack();
        }

        private void switchWorldCommand()
        {
            if (Input.GetButtonDown(_settings.worldSwitchButton))
                _inputEvents.OnSwitch();
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