using System;
using UnityEngine;
using Zenject;

namespace GG
{
    public class Pushback : IInitializable, ITickable
    {
        public Action OnPushbackStart;
        public Action OnPushbackEnd;

        private CharacterMotor _motor;
        private Life _life;
        private Settings _settings;

        private bool _isPushingback;

        public Pushback(CharacterMotor motor, Life life, Settings settings)
        {
            _motor = motor;
            _life = life;
            _settings = settings;
        }

        public void Initialize()
        {
            _life.OnTakeDamageFromPoint += onTakeDamageFromObject;
        }

        public void Tick()
        {
            if (_motor.IsGrounded && _motor.Velocity.y <= 0 && _isPushingback)
            {
                _isPushingback = false;
                if (OnPushbackEnd != null)
                    OnPushbackEnd();
            }
        }

        private void onTakeDamageFromObject(int amount, Vector2 sourcePosition)
        {
            var pushbackDirection = sourcePosition - _motor.Position;

            var pushBackVelocity = new Vector2(-Mathf.Sign(pushbackDirection.x) * _settings.force.x, _settings.force.y);

            _motor.SetVelocity(pushBackVelocity);

            _isPushingback = true;

            if (OnPushbackStart != null)
                OnPushbackStart();
        }

        [System.Serializable]
        public class Settings
        {
            public Vector2 force;
        }
    }
}