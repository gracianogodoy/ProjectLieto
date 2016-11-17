using Zenject;
using Gamelogic;
using Gamelogic.Extensions;
using UnityEngine;
using System;

namespace GG
{
    public class SquireAI : IInitializable, ITickable
    {
        public enum State
        {
            Idle,
            Moving,
            Dead,
            TakingHit
        }

        private Move _move;
        private FaceDirection _faceDirection;
        private Settings _settings;
        private CharacterMotor _motor;
        private Pushback _pushback;

        private StateMachine<State> _stateMachine = new StateMachine<State>();
        private Clock _clock = new Clock();
        private float _moveDistance;
        private float _moveCurrentDistance;
        private float _lastPositionX;

        public SquireAI(FaceDirection faceDirection, Move move, Settings settings, CharacterMotor motor, Pushback pushback)
        {
            _faceDirection = faceDirection;
            _move = move;
            _settings = settings;
            _motor = motor;
            _pushback = pushback;
        }

        public void Initialize()
        {
            _stateMachine.AddState(State.Idle, enterIdle, updateIdle);
            _stateMachine.AddState(State.Moving, enterMoving, updateMoving);
            _stateMachine.AddState(State.Dead);
            _stateMachine.AddState(State.TakingHit);

            _stateMachine.CurrentState = State.Idle;

            _pushback.OnPushbackStart += () => _stateMachine.CurrentState = State.TakingHit;
            _pushback.OnPushbackEnd += () => _stateMachine.CurrentState = State.Idle;
        }

        public void Tick()
        {
            _stateMachine.Update();
        }

        public void SetState(State newState)
        {
            _stateMachine.CurrentState = newState;
        }

        #region Idle State
        private void enterIdle()
        {
            _clock.Reset(_settings.idleTime);
            _clock.Unpause();
        }

        private void updateIdle()
        {
            _clock.Update(Time.deltaTime);

            if (_clock.IsDone)
            {
                _stateMachine.CurrentState = State.Moving;
            }
        }
        #endregion

        #region Moving State
        private void enterMoving()
        {
            randomizeFaceDirection();
            _moveCurrentDistance = 0;
            _moveDistance = UnityEngine.Random.Range(_settings.minMoveDistance, _settings.maxMoveDistance);
            _lastPositionX = _motor.LocalPosition.x;
        }

        private void updateMoving()
        {
            _moveCurrentDistance += Mathf.Abs(_motor.LocalPosition.x - _lastPositionX);
            _lastPositionX = _motor.LocalPosition.x;

            _move.OnMove(_faceDirection.Direction);

            if (_moveCurrentDistance > _moveDistance)
            {
                _stateMachine.CurrentState = State.Idle;
            }

            CheckChangeDirection.Check(_faceDirection, _motor.Position, 1, 0.3f, "SquireChangeDirection");
        }
        #endregion

        private void randomizeFaceDirection()
        {
            var direction = UnityEngine.Random.Range(0, 1.0f) < 0.5f ? -1 : 1;
            _faceDirection.SetDirection(direction);
        }

        private void onTakeDamage(int damage)
        {
            _stateMachine.CurrentState = State.TakingHit;
        }

        [System.Serializable]
        public class Settings
        {
            public float idleTime;
            public float minMoveDistance;
            public float maxMoveDistance;
        }
    }
}