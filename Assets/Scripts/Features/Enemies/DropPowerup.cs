using UnityEngine;
using Zenject;

namespace GG
{
    public class DropPowerup : IInitializable
    {
        private Life _life;
        private Settings _settings;
        private CharacterMotor _motor;
        private ConsumePowerup.Settings _powerupSettings;

        public DropPowerup(Life life, Settings settings, CharacterMotor motor, ConsumePowerup.Settings powerupSettings)
        {
            _life = life;
            _settings = settings;
            _motor = motor;
            _powerupSettings = powerupSettings;
        }

        public void Initialize()
        {
            _life.OnDead += onDead;
            Random.InitState(1000);
        }

        private void onDead()
        {
            var willDrop = Random.Range(0, 101) <= _settings.dropChance;

            if (willDrop)
            {
                var dropAmount = Random.Range(_settings.minDropAmount, _settings.maxDropAmount + 1);

                for (int i = 0; i < dropAmount; i++)
                {
                    dropPowerup();
                }
            }
        }

        private void dropPowerup()
        {
            var powerup = GameObject.Instantiate(_powerupSettings.powerupPrefab, (Vector3)_motor.Position, Quaternion.identity) as GameObject;

            var rigidbody = powerup.GetComponent<Rigidbody2D>();
            powerup.GetComponent<Powerup>().PowerupType = Powerup.Type.Dropped;

            var randomX = Random.Range(_powerupSettings.minDropForce.x, _powerupSettings.maxDropForce.x);
            var randomY = Random.Range(_powerupSettings.minDropForce.y, _powerupSettings.maxDropForce.y);

            var force = new Vector2(randomX, randomY);

            rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        [System.Serializable]
        public class Settings
        {
            [Range(0, 100)]
            public int dropChance;
            [Range(0, 5)]
            public int minDropAmount;
            [Range(0, 5)]
            public int maxDropAmount;
        }
    }
}