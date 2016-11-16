using Gamelogic.Extensions;
using System;
using UnityEngine;
using Zenject;

namespace GG
{
    public class GhoulDeath : IInitializable, ITickable
    {
        private GameObject _owner;
        private Settings _settings;
        private Life _life;
        private Attack _attack;
        private FaceDirection _faceDirection;
        private GhoulAI _ghoulAi;

        private Clock _clock = new Clock();

        public GhoulDeath([Inject(Id = InjectId.Owner)] GameObject owner, Settings settings, 
            Life life, Attack attack, FaceDirection faceDirection, GhoulAI ghoulAi)
        {
            _owner = owner;
            _settings = settings;
            _life = life;
            _faceDirection = faceDirection;
            _attack = attack;
            _ghoulAi = ghoulAi;
        }

        public void Initialize()
        {
            _life.OnDead += onDead;
        }

        public void Tick()
        {
            _clock.Update(Time.deltaTime);

            if (_clock.IsDone)
                GameObject.Destroy(_owner);
        }

        private void onDead()
        {
            _attack.SetEnable(false);
            _faceDirection.SetEnable(false);
            _ghoulAi.SetEnable(false);

            _clock.Reset(_settings.timeToDestroy);
            _clock.Unpause();
        }

        [Serializable]
        public class Settings
        {
            public float timeToDestroy;
        }
    }
}