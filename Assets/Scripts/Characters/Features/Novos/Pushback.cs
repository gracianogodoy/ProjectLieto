using System;
using UnityEngine;
using Zenject;

namespace GG
{
    public class Pushback : IInitializable
    {
        private CharacterMotor _motor;
        private Life _life;
        private Settings _settings;

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

        private void onTakeDamageFromObject(int amount, Vector2 sourcePosition)
        {
            var pushbackDirection = sourcePosition - _motor.Position;

            var pushBackVelocity = new Vector2(Mathf.Sign(pushbackDirection.x) * _settings.force.x, _settings.force.y);

            _motor.SetVelocity(pushBackVelocity);
        }

        [System.Serializable]
        public class Settings
        {
            public Vector2 force;
        }
    }
}