using UnityEngine;
using Zenject;

namespace GG
{
    public class InputCommands
    {
        public class MoveCommand : Command<int> { }
        public class JumpCommand : Command { }
        public class StopJumpCommand : Command { }
        public class AttackCommand : Command { }
        public class SwitchCommand : Command { }
    }

    public class InputHandler : ITickable
    {
        private Settings _settings;

        public InputCommands.MoveCommand OnMove;
        public InputCommands.JumpCommand OnJump;
        public InputCommands.StopJumpCommand OnStopJump;
        public InputCommands.AttackCommand OnAttack;
        public InputCommands.SwitchCommand OnSwitch;

        public InputHandler(Settings settings, InputCommands.MoveCommand move,
            InputCommands.JumpCommand jump, InputCommands.StopJumpCommand stopJump,
            InputCommands.AttackCommand attack, InputCommands.SwitchCommand switchCommand)
        {
            _settings = settings;
            OnMove = move;
            OnJump = jump;
            OnStopJump = stopJump;
            OnAttack = attack;
            OnSwitch = switchCommand;
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

            OnMove.Execute((int)axis);
        }

        private void jumpCommand()
        {
            if (Input.GetButtonDown(_settings.jumpButton))
                OnJump.Execute();

            if (Input.GetButtonUp(_settings.jumpButton))
                OnStopJump.Execute();
        }

        private void attackCommand()
        {
            if (Input.GetButtonDown(_settings.attackButton))
                OnAttack.Execute();
        }

        private void switchWorldCommand()
        {
            if (Input.GetButtonDown(_settings.worldSwitchButton))
                OnSwitch.Execute();
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