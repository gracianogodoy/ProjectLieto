using UnityEngine;
using Zenject;

namespace GG
{
    public class SquireSoundController : MonoBehaviour
    {
        public AudioClip squireDeathSound;

        private Life _life;

        [Inject]
        public void Construct(Life life)
        {
            _life = life;
        }

        void Start()
        {
            _life.OnDead += onDead;
        }

        private void onDead()
        {
            SoundKit.instance.playSound(squireDeathSound, 0.1f, 1, 0);
        }
    }
}