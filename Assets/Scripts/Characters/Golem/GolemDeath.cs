using System;
using Zenject;

namespace GG
{
    public class GolemDeath : IInitializable
    {
        private Life _life;
        private EnemyAttackAI _attackAi;
        private ProximitySensor _sensor;
        private FaceDirection _faceDirection;

        public GolemDeath(FaceDirection faceDirection, Life life, ProximitySensor sensor, EnemyAttackAI attackAi)
        {
            _life = life;
            _sensor = sensor;
            _attackAi = attackAi;
            _faceDirection = faceDirection;
        }

        public void Initialize()
        {
            _life.OnDead += onDead;
            _life.OnResurrect += onResurrect;
        }

        private void onResurrect()
        {
            setEnable(true);
            _faceDirection.Reset();
        }

        private void onDead()
        {
            setEnable(false);
        }

        private void setEnable(bool value)
        {
            _attackAi.SetEnable(value);
            _sensor.enabled = value;
            _faceDirection.SetEnable(value);
        }
    }
}