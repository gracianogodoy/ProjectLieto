using System;
using Zenject;

namespace GG
{
    public class GolemDeath : IInitializable
    {
        private Life _life;
        private EnemyAttackAI _attackAi;
        private ProximitySensor _sensor;

        public GolemDeath(Life life, ProximitySensor sensor, EnemyAttackAI attackAi)
        {
            _life = life;
            _sensor = sensor;
            _attackAi = attackAi;
        }

        public void Initialize()
        {
            _life.OnDead += onDead;
        }

        private void onDead()
        {
            _attackAi.SetEnable(false);
            _sensor.enabled = false;
        }
    }
}