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
        public Action OnRessurect { get; set; }
        public Action<int> OnTakeDamage { get; set; }
        public Action<int, Vector2> OnTakeDamageFromPoint { get; set; }

        public int CurrentLife
        {
            get
            {
                return _currentLife;
            }
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
            damage(amount, Vector2.zero);
        }

        public void TakeDamage(int amount, Vector2 damageSourcePoint)
        {
            damage(amount, damageSourcePoint);
        }

        public void Heal(int amount)
        {
            changeLife(amount);
        }

        public void Ressurect()
        {
            if (_currentLife > 0)
                return;

            _currentLife = _settings.TotalLife;

            if (OnRessurect != null)
                OnRessurect();
        }

        private void dead()
        {
            if (OnDead != null)
                OnDead();
        }

        private void damage(int amount, Vector2 damageSourcePoint)
        {
            if (_currentLife <= 0)
                return;

            changeLife(-amount);

            if (OnTakeDamageFromPoint != null)
                OnTakeDamageFromPoint(amount, damageSourcePoint);

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