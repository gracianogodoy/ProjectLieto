using UnityEngine;
using UnityEngine.Assertions;

namespace GG
{
    public class GhoulAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Attack _attack;
        private Life _life;

        private bool _doAttack;

        [Zenject.Inject]
        public void Construct(Attack attack, Life life)
        {
            _attack = attack;
            _life = life;
        }

        void Start()
        {
            _animator = GetComponent<Animator>();
            Assert.IsNotNull(_animator);

            _life.OnDead += onDead;
            _life.OnRessurect += () => { _animator.SetTrigger("Alive"); };
        }

        void Update()
        {
            if (_attack.IsAttacking && !_doAttack)
            {
                _animator.SetTrigger("Attack");
                _doAttack = true;
            }

            if (!_attack.IsAttacking && _doAttack)
                _doAttack = false;
        }

        private void onDead()
        {
            _animator.SetTrigger("Die");
        }
    }
}