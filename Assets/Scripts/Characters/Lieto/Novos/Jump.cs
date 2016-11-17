using Zenject;
using System;

namespace GG
{
    public class Jump : BaseCharacterBehaviour, ITickable
    {
        public Action OnJump;
        public Action OnStopJump;

        private Settings _settings;
        private CharacterMotor _motor;

        private bool _isJumping;

        public bool IsJumping
        {
            get
            {
                return _isJumping;
            }
        }

        public Jump(CharacterMotor motor, Settings setings)
        {
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
                if (OnStopJump != null)
                    OnStopJump();
            }
        }

        public void StartJump()
        {
            if (_motor.IsGrounded && _isEnable)
            {
                _isJumping = true;
                if (OnJump != null)
                    OnJump();

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