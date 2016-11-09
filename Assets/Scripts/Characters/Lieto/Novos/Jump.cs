using Zenject;
using System;

namespace GG
{
    public class Jump : BaseCharacterBehaviour, ITickable
    {
        public class JumpStartCommand : Command { }
        public class JumpEndCommand : Command { }

        private JumpStartCommand _startCommand;
        private JumpEndCommand _endCommand;
        private Settings _settings;

        private bool _isJumping;

        public Jump(JumpStartCommand start, JumpEndCommand end, CharacterMotor motor, Settings setings) : base(motor)
        {
            _startCommand = start;
            _endCommand = end;
            _motor = motor;
            _settings = setings;
        }

        public void Tick()
        {
            if (!_isJumping || !_isEnable)
                return;

            if (_motor.CollisionState.above)
            {
                _motor.SetVelocityY(0);
                _isJumping = false;
            }

            var fallingDown = _motor.Velocity.y < 0;

            if (_motor.IsGrounded && _isJumping && fallingDown)
            {
                _isJumping = false;
                _endCommand.Execute();
            }
        }

        public void StartJump()
        {
            if (_motor.IsGrounded && _isEnable)
            {
                _isJumping = true;
                _startCommand.Execute();
                _motor.SetVelocityY(_settings.jumpPower);
            }
        }

        public void StopJump()
        {
            if (!_motor.IsGrounded && _motor.Velocity.y > _settings.shortJumpPower && _isEnable)
            {
                _motor.SetVelocityY(_settings.shortJumpPower);
            }
        }

        [Serializable]
        public class Settings
        {
            public float jumpPower;
            public float shortJumpPower;
        }
    }
}