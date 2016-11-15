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

        private Clock _clock = new Clock();

        public GhoulDeath([Inject(Id = InjectId.Owner)] GameObject owner, Settings settings, Life life)
        {
            _owner = owner;
            _settings = settings;
            _life = life;
        }

        public void Initialize()
        {
            _life.OnDead += onDead;
        }

        public void Tick()
        {

        }

        private void onDead()
        {

        }

        [Serializable]
        public class Settings
        {
            public float timeToDestroy;
        }
    }
}