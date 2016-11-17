using System;
using UnityEngine;
using Zenject;

namespace GG
{
    public class GhoulDeath : IInitializable
    {
        private GameObject _owner;
        private Settings _settings;
        private Life _life;
        private Attack _attack;
        private FaceDirection _faceDirection;
        private GhoulAI _ghoulAi;
        private ProximitySensor _sensor;

        public GhoulDeath([Inject(Id = InjectId.Owner)] GameObject owner, Settings settings,
            Life life, Attack attack, FaceDirection faceDirection, GhoulAI ghoulAi, ProximitySensor sensor)
        {
            _owner = owner;
            _settings = settings;
            _life = life;
            _faceDirection = faceDirection;
            _attack = attack;
            _ghoulAi = ghoulAi;
            _sensor = sensor;
        }

        public void Initialize()
        {
            _life.OnDead += onDead;
            _life.OnResurrect += onRessurect;
        }

        private void onRessurect()
        {
            setEnables(true);
            _faceDirection.Reset();
        }

        private void onDead()
        {
            setEnables(false);
        }

        private void setEnables(bool value)
        {
            _attack.SetEnable(value);
            _faceDirection.SetEnable(value);
            _sensor.enabled = value;
            _ghoulAi.SetState(GhoulAI.State.Dead);
        }

        [Serializable]
        public class Settings
        {
            public float timeToDestroy;
        }
    }
}