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

        private bool _isPushed;
        private Vector2 _lastPosition;
        private float _distanceMoved;

        public Pushback(CharacterMotor motor, Life life, Settings settings)
        {
            _motor = motor;
            _life = life;
            _settings = settings;
        }

        public void Initialize()
        {
            _life.OnTakeDamageFromPoint += onTakeDamageFromPoint;
        }

        public void Tick()
        {
            if (_motor.IsGrounded && _motor.Velocity.y <= 0 && _isPushed)
            {
                _isPushed = false;
                if (OnPushbackEnd != null)
                    OnPushbackEnd();
            }

            //if (_isPushed)
            //{
            //    _distanceMoved += Mathf.Abs(_motor.Position.x - _lastPosition.x);
            //    Debug.Log(_distanceMoved);
            //    _lastPosition = _motor.Position;
            //    if (_distanceMoved >= _settings.force.x)
            //    {
            //        _isPushed = false;
            //        if (OnPushbackEnd != null)
            //            OnPushbackEnd();
            //    }
            //}
        }

        private void onTakeDamageFromPoint(int amount, Vector2 direction)
        {
            _lastPosition = _motor.Position;
            _distanceMoved = 0;
            var pushbackDirection = direction;

            var pushBackVelocity = new Vector2(Mathf.Sign(pushbackDirection.x) * _settings.force.x, _settings.force.y);

            _motor.SetVelocity(pushBackVelocity);

            _isPushed = true;

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