using UnityEngine;
using Zenject;

namespace GG
{
    public class Hazard : ITickable
    {
        private Life _life;
        private CharacterMotor _motor;
        private Settings _settings;

        public Hazard(Life life, CharacterMotor motor, Settings settings)
        {
            _life = life;
            _motor = motor;
            _settings = settings;
        }

        public void Tick()
        {
            checkForHazard();
        }

        private void checkForHazard()
        {
            var position = new Vector2(_motor.Position.x, _motor.Position.y);
            var direction = Vector2.down;

            var hits = Physics2D.RaycastAll(position, direction, 0.2f, _settings.hazardLayer);

            foreach (var hit in hits)
            {
                _life.TakeDamage(20, -direction);
            }
        }

        [System.Serializable]
        public class Settings
        {
            public LayerMask hazardLayer;
        }
    }
}