using UnityEngine;
using Zenject;

namespace GG
{
    public class ReceiveDamage : IInitializable
    {
        DetectStrike _detectStrike;
        Life _life;

        public ReceiveDamage(Life life, DetectStrike detectStrike)
        {
            _life = life;
            _detectStrike = detectStrike;
        }

        public void Initialize()
        {
            _detectStrike.OnStrike += onStrike;
        }

        private void onStrike(int damage, Vector2 direction)
        {
            if (direction.x == 0 && direction.y == 0)
                _life.TakeDamage(damage);
            else
                _life.TakeDamage(damage, direction);
        }
    }
}