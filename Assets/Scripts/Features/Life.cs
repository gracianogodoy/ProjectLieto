using UnityEngine;
using System;
using Zenject;

namespace GG
{
    public class Life : IInitializable
    {
        private Settings _settings;
        private int _currentLife;

        public Action OnDead { get; set; }
        public Action OnResurrect { get; set; }
        public Action<int> OnTakeDamage { get; set; }
        public Action<int> OnHeal { get; set; }
        public Action<int, Vector2> OnTakeDamageFromPoint { get; set; }

        public int CurrentLife
        {
            get
            {
                return _currentLife;
            }
        }

        public bool IsFull
        {
            get { return _currentLife >= _settings.TotalLife; }
        }

        public Life(Settings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            _currentLife = _settings.TotalLife;
        }

        public void TakeDamage(int amount)
        {
            damage(amount, Vector2.zero, false);
        }

        public void TakeDamage(int amount, Vector2 direction)
        {
            damage(amount, direction, true);
        }

        public void Heal(int amount)
        {
            if (IsFull)
                return;

            changeLife(amount);

            if (OnHeal != null)
                OnHeal(amount);
        }

        public void Ressurect()
        {
            if (_currentLife > 0)
                return;

            _currentLife = _settings.TotalLife;

            if (OnResurrect != null)
                OnResurrect();
        }

        private void dead()
        {
            if (OnDead != null)
                OnDead();
        }

        private void damage(int amount, Vector2 direction, bool hasPoint)
        {
            if (_currentLife <= 0)
                return;

            changeLife(-amount);

            if (OnTakeDamageFromPoint != null && hasPoint)
                OnTakeDamageFromPoint(amount, direction);

            if (OnTakeDamage != null)
                OnTakeDamage(amount);

            if (_currentLife <= 0)
                dead();
        }

        private void changeLife(int amount)
        {
            _currentLife += amount;

            _currentLife = Mathf.Clamp(_currentLife, 0, _settings.TotalLife);
        }

        [Serializable]
        public class Settings
        {
            public int TotalLife;
        }
    }
}