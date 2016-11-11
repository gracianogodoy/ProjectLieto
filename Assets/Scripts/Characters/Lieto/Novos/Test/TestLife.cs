using System;
using UnityEngine;
using Zenject;

namespace GG
{
    public class TestLife : ITickable, IInitializable
    {
        private Life _life;
        public TestLife(Life life)
        {
            _life = life;
        }

        public void Initialize()
        {
            _life.OnTakeDamage += onTakeDamage;
            _life.OnDead += onDead;
        }

        private void onDead()
        {
            Debug.Log("DEAD!");
        }

        private void onTakeDamage(int damage)
        {
            Debug.Log("Take damage: " + damage + ". Life is now " + _life.CurrentLife);
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.T))
                _life.TakeDamage(1);
        }
    }
}