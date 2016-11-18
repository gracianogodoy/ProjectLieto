using UnityEngine;
using Zenject;

namespace GG
{
    public class ConsumePowerup : IInitializable
    {
        private DetectPowerup _detectPowerup;
        private Settings _settings;
        private Life _life;

        public ConsumePowerup(DetectPowerup detectPowerup, Settings settings, Life life)
        {
            _detectPowerup = detectPowerup;
            _settings = settings;
            _life = life;
        }

        public void Initialize()
        {
            _detectPowerup.OnDetect += onDetectPowerup;
        }

        private void onDetectPowerup()
        {
            _life.Heal(_settings.lifeRestoredAmout);
        }

        [System.Serializable]
        public class Settings
        {
            public GameObject powerupPrefab;
            public Vector2 maxDropForce;
            public Vector2 minDropForce;
            public int lifeRestoredAmout;
        }
    }
}