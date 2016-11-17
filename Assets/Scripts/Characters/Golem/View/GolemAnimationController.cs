using UnityEngine;
using UnityEngine.Assertions;

namespace GG
{
    public class GolemAnimationController : MonoBehaviour
    {
        private ProximitySensor _sensor;
        private Animator _animator;
        private Attack _attack;
        private Life _life;

        private bool _doAttack;

        [Zenject.Inject]
        public void Construct(Life life, Attack attack)
        {
            _attack = attack;
            _life = life;
        }

        void Start()
        {
            _sensor = GetComponentInParent<ProximitySensor>();
            Assert.IsNotNull(_sensor);

            _animator = GetComponent<Animator>();
            Assert.IsNotNull(_animator);

            _life.OnDead += onDead;

            _sensor.ReadySensor.OnEnterSensor += onEnterReadySensor;
            _sensor.ReadySensor.OnLeaveSensor += onLeaveReadySensor;
            _life.OnResurrect += () => { _animator.SetTrigger("Alive"); };
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

        private void onEnterReadySensor(GameObject target)
        {
            _animator.SetBool("IsReady", true);
        }

        private void onLeaveReadySensor(GameObject target)
        {
            _animator.SetBool("IsReady", false);
        }

        private void onDead()
        {
            _animator.SetTrigger("Die");
        }
    }
}