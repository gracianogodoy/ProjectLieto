using UnityEngine;
using Zenject;

namespace GG
{
    [SelectionBase]
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
            _life.OnResurrect += () => { _animator.SetTrigger("Alive"); };
        }

        void Update()
        {
            //if (name == "squire 2") 
            //Debug.Log(_motor.Velocity.x);
            _animator.SetBool("IsWalking", Mathf.Abs(_motor.Velocity.x) > 0f);
        }

        private void onDead()
        {
            _animator.SetTrigger("Die");
        }
    }

}