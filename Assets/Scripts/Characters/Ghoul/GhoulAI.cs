using Gamelogic.Extensions;
using System;
using UnityEngine;
using Zenject;

namespace GG
{
    public class GhoulAI : IInitializable, ITickable
    {
        public enum State
        {
            Patrolling,
            Chasing,
            Reseting,
            Pushback,
            Dead
        }

        private Settings _settings;

        private ProximitySensor _sensor;
        private CharacterMotor _motor;
        private FaceDirection _faceDirection;
        private Pushback _pushback;
        private Vector2 _initialPosition;

        private Transform _target;

        private StateMachine<State> _stateMachine = new StateMachine<State>();
        private State _lastState;

        public GhoulAI(Settings settings, ProximitySensor sensor, CharacterMotor motor, FaceDirection faceDirection, Pushback pushback)
        {
            _sensor = sensor;
            _motor = motor;
            _faceDirection = faceDirection;
            _settings = settings;
            _pushback = pushback;
        }

        public void Initialize()
        {
            _sensor.ReadySensor.OnEnterSensor += onEnterSensor;
            _sensor.ReadySensor.OnLeaveSensor += onLeaveSensor;

            _stateMachine.AddState(State.Patrolling, null, updatePatrolling);
            _stateMachine.AddState(State.Chasing, null, updateChasing);
            _stateMachine.AddState(State.Reseting, null, updateReseting);
            _stateMachine.AddState(State.Pushback);
            _stateMachine.AddState(State.Dead, enterDead);

            _stateMachine.CurrentState = State.Patrolling;

            _initialPosition = _motor.Position;

            //_pushback.OnPushbackStart += () =>
            //{
            //    _lastState = _stateMachine.CurrentState;
            //    _stateMachine.CurrentState = State.Pushback;
            //};

            //_pushback.OnPushbackEnd += () =>
            //{
            //    _stateMachine.CurrentState = _lastState;
            //};
        }

        public void Tick()
        {
            _stateMachine.Update();
        }

        public void SetState(State newState)
        {
            _stateMachine.CurrentState = newState;
        }

        private void onLeaveSensor(GameObject target)
        {
            _stateMachine.CurrentState = State.Reseting;
            _target = null;
        }

        private void onEnterSensor(GameObject target)
        {
            _stateMachine.CurrentState = State.Chasing;
            _target = target.transform;
        }

        #region Patrolling State
        private void updatePatrolling()
        {
            patrolling();

            CheckChangeDirection.Check(_faceDirection, _motor.Position, 1, "ChangeGhoulDirection");
        }

        private void patrolling()
        {
            _motor.SetVelocity(new Vector2(_faceDirection.Direction, 0) * _settings.patrolSpeed);
        }
        #endregion

        #region Chasing State
        private void updateChasing()
        {
            if (_target == null)
                return;

            var _direction = (Vector2)_target.transform.position - _motor.Position;

            if (Vector2.Distance(_motor.Position, _target.transform.position) <= _settings.distanceToStop)
            {
                _motor.SetVelocity(Vector2.zero);
            }
            else
            {
                _motor.SetVelocity(_direction.normalized * _settings.chaseSpeed);
            }
        }
        #endregion

        #region Reseting State
        private void updateReseting()
        {
            backToInitialPosition();
        }

        private void backToInitialPosition()
        {
            if (Vector2.Distance(_motor.Position, _initialPosition) >= 0.1f)
            {
                var direction = _initialPosition - _motor.Position;
                _faceDirection.SetDirection((int)Mathf.Sign(direction.x));

                _motor.SetVelocity(direction.normalized * _settings.patrolSpeed);
            }
            else
            {
                _stateMachine.CurrentState = State.Patrolling;
            }
        }
        #endregion

        #region Dead State
        private void enterDead()
        {
            _motor.SetVelocity(Vector2.zero);
        }
        #endregion

        [System.Serializable]
        public class Settings
        {
            public float chaseSpeed;
            public float patrolSpeed;
            public float distanceToStop;
        }
    }
}