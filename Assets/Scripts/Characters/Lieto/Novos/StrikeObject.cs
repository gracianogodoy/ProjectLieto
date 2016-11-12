using UnityEngine;
using Zenject;

namespace GG
{
    public class Strike : IInitializable
    {
        private Attack _attack;
        private Settings _settings;
        private GameObject _owner;

        public Strike(Attack attack, Settings settings, [Inject(Id = InjectId.Owner)]  GameObject owner)
        {
            _attack = attack;
            _settings = settings;
            _owner = owner;
        }

        public void Initialize()
        {
            _attack.OnAttackHit += onAttackHit;
        }

        private void onAttackHit(GameObject[] hits)
        {
            foreach (var gameObject in hits)
            {
                var strikeable = gameObject.GetComponent<IStrikeable>();

                if (strikeable != null)
                    strikeable.Striked(_settings.Damage, _owner);
            }
        }

        [System.Serializable]
        public class Settings
        {
            public int Damage;
        }
    }
}