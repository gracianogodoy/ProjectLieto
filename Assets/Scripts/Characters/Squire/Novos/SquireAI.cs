﻿using Zenject;
using Gamelogic;
using Gamelogic.Extensions;
using UnityEngine;

namespace GG
{
    public class SquireAI : IInitializable, ITickable
    {
        public enum States
        {
            Idle,
            Moving,
        }

        private Move _move;
        private FaceDirection _faceDirection;
        private Settings _settings;
        private CharacterMotor _motor;

        private StateMachine<States> _stateMachine = new StateMachine<States>();
        private Clock _clock = new Clock();
        private float _moveDistance;
        private float _moveCurrentDistance;
        private float _lastPositionX;

        public SquireAI(FaceDirection faceDirection, Move move, Settings settings, CharacterMotor motor)
        {
            _faceDirection = faceDirection;
            _move = move;
            _settings = settings;
            _motor = motor;
        }

        public void Initialize()
        {
            _stateMachine.AddState(States.Idle, enterIdle, updateIdle);
            _stateMachine.AddState(States.Moving, enterMoving, updateMoving);

            _stateMachine.CurrentState = States.Idle;
        }

        public void Tick()
        {
            _stateMachine.Update();
        }

        #region Idle State
        private void enterIdle()
        {
            _move.OnMove(0);
            _clock.Reset(_settings.idleTime);
            _clock.Unpause();
        }

        private void updateIdle()
        {
            _clock.Update(Time.deltaTime);

            if (_clock.IsDone)
            {
                _stateMachine.CurrentState = States.Moving;
            }
        }
        #endregion

        #region Moving State
        private void enterMoving()
        {
            randomizeFaceDirection();
            _moveCurrentDistance = 0;
            _moveDistance = UnityEngine.Random.Range(_settings.minMoveDistance, _settings.maxMoveDistance);
            _lastPositionX = _motor.Position.x;
        }

        private void updateMoving()
        {
            _moveCurrentDistance += Mathf.Abs(_motor.Position.x - _lastPositionX);
            _lastPositionX = _motor.Position.x;

            _move.OnMove(_faceDirection.Direction);

            if (_moveCurrentDistance > _moveDistance)
            {
                _stateMachine.CurrentState = States.Idle;
            }
        }
        #endregion

        private void randomizeFaceDirection()
        {
            var direction = UnityEngine.Random.Range(0, 1.0f) < 0.5f ? -1 : 1;
            _faceDirection.SetDirection(direction);
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