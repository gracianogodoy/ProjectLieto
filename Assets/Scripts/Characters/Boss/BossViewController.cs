using UnityEngine;
using Zenject;

namespace GG
{
    public class BossViewController : MonoBehaviour
    {
        [Inject]
        private Life _life;
        [Inject]
        private BossAI _ai;

        private Animator _animator;

        void Start()
        {
            _animator = GetComponent<Animator>();

            _life.OnDead += onDead;
            _life.OnTakeDamage += onTakeDamage;
        }

        void Update()
        {
            if (_ai.CurrentState == BossAI.State.Attacking)
                _animator.SetBool("IsAttacking", true);

            if (_ai.CurrentState == BossAI.State.Idle)
                _animator.SetBool("IsAttacking", false);
        }

        private void onTakeDamage(int damage)
        {
            if (_life.CurrentLife > 0)
                _animator.SetTrigger("Hit");
        }

        private void onDead()
        {
            _animator.SetTrigger("Dead");
        }
    }

}