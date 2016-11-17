using UnityEngine;
using System.Collections;

namespace GG
{
    public class DamageBlink : MonoBehaviour
    {
        private Settings _settings;
        private Life _life;
        private bool _isBlinking;
        private SpriteRenderer _renderer;

        [Zenject.Inject]
        public void Construct(Life life, Settings settings)
        {
            _life = life;
            _settings = settings;
        }

        void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _life.OnTakeDamage += onTakeDamage;
        }

        private void onTakeDamage(int amount)
        {
            if (!_isBlinking)
            {
                _isBlinking = true;
                StartCoroutine(blink());
            }
        }

        private IEnumerator blink()
        {
            var currentBlink = 0;
            var changeColor = false;

            while (currentBlink < _settings.numberOfBlinks)
            {
                _renderer.color = changeColor ? Color.white : _settings.blinkColor;

                yield return new WaitForSeconds(_settings.intervalBetweenBlinks);

                if (!changeColor)
                    currentBlink++;

                changeColor = !changeColor;
            }

            _renderer.color = Color.white;
            _isBlinking = false;
        }

        [System.Serializable]
        public class Settings
        {
            public Color blinkColor = Color.red;
            public float numberOfBlinks = 3;
            public float intervalBetweenBlinks = 0.01f;
        }
    }
}
