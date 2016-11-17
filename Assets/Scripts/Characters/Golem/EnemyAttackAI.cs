using Gamelogic.Extensions;
using Zenject;
using UnityEngine;

namespace GG
{
    public class EnemyAttackAI : BaseCharacterBehaviour, IInitializable, ITickable
    {
        public enum State
        {
            Idle,
            Attacking,
        }

        private ProximitySensor _sensor;
        private Attack _attack;
        private Settings _settings;
        private GameObject _owner;
        private FaceDirection _faceDirection;

        private StateMachine<State> _stateMachine = new StateMachine<State>();
        private Clock _clock = new Clock();

        public EnemyAttackAI(ProximitySensor sensor, Attack attack, Settings settings, FaceDirection faceDirection, [Inject(Id = InjectId.Owner)] GameObject owner)
        {
            _sensor = sensor;
            _attack = attack;
            _settings = settings;
            _owner = owner;
            _faceDirection = faceDirection;
        }

        public void Initialize()
        {
            _stateMachine.AddState(State.Idle);
            _stateMachine.AddState(State.Attacking, enterAttacking, updateAttacking);

            _sensor.ReadySensor.OnInsideSensor += onInsideReadySensor;

            _sensor.AttackSensor.OnEnterSensor += onEnterAttackSensor;
            _sensor.AttackSensor.OnLeaveSensor += onLeaveAttackSensor;

            _stateMachine.CurrentState = State.Idle;
        }

        public void Tick()
        {
            if (!_isEnable)
                return;

            _clock.Update(Time.deltaTime);

            if (_clock.IsDone)
            {
                _clock.Pause();
            }

            _stateMachine.Update();
        }

        #region Sensor Callbacks
        private void onEnterAttackSensor(GameObject target)
        {
            _stateMachine.CurrentState = State.Attacking;
        }

        private void onLeaveAttackSensor(GameObject target)
        {
            _stateMachine.CurrentState = State.Idle;
        }

        private void onInsideReadySensor(GameObject target)
        {
            var direction = (target.transform.localPosition - _owner.transform.localPosition).normalized;

            _faceDirection.SetDirection((int)Mathf.Sign(direction.x));
        }
        #endregion

        #region Attack State
        private void enterAttacking()
        {
            if (_clock.IsPaused)
            {
                _attack.OnAttack();
                resetClock();
            }
        }

        private void updateAttacking()
        {
            if (_clock.IsDone)
            {
                _attack.OnAttack();
                resetClock();
            }
        }

        private void resetClock()
        {
            _clock.Reset(_settings.timeBetweenAttacks);
            _clock.Unpause();
        }
        #endregion

        [System.Serializable]
        public class Settings
        {
            public float timeBetweenAttacks;
        }
    }
}