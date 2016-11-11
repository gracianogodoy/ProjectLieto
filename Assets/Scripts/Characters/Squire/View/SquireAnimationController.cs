using UnityEngine;
using Zenject;

namespace GG
{
    public class SquireAnimationController : MonoBehaviour
    {
        private Animator _animator;

        [Inject]
        private CharacterMotor _motor;
        [Inject]
        private Life _life;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _life.OnDead += onDead;
        }

        void Update()
        {
            _animator.SetBool("IsWalking", Mathf.Abs(_motor.Velocity.x) > 0f);
        }

        private void onDead()
        {
            _animator.SetTrigger("Die");
        }
    }

}