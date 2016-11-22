using Zenject;
using Gamelogic.Extensions;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine;

namespace GG
{
    public class BossAI : IInitializable
    {
        [Inject]
        private BossFightStartSignal _bossFightStartSignal;
        private Settings _settings;

        public State CurrentState
        {
            get { return _stateMachine.CurrentState; }
        }

        public BossAI(Settings settings)
        {
            _settings = settings;
        }

        public enum State
        {
            Attacking,
            Waiting,
            Idle,
        }

        private StateMachine<State> _stateMachine = new StateMachine<State>();

        public void Initialize()
        {
            _stateMachine.AddState(State.Idle);
            _stateMachine.AddState(State.Waiting);
            _stateMachine.AddState(State.Attacking, enterAttack);

            _stateMachine.CurrentState = State.Waiting;

            _bossFightStartSignal += bossFightStart;
        }

        private void bossFightStart()
        {
            _stateMachine.CurrentState = State.Attacking;
        }

        private void enterIdle()
        {

        }

        #region Attack State
        private void enterAttack()
        {
            Timing.RunCoroutine(startAttack());
        }

        private IEnumerator<float> startAttack()
        {
            yield return Timing.WaitForSeconds(_settings.timeToStartAttack);

            var numberOfPowers = Random.Range(_settings.minPowerNumber, _settings.maxPowerNumber + 1);
            var spaceBetween = Mathf.Abs(_settings.powerAreaLeftPosition - _settings.powerAreaRightPosition) / (_settings.numberOfPowers -1);
            var position = new Vector2( _settings.powerAreaLeftPosition, _settings.powerYPosition);

            for (int i = 0; i < _settings.numberOfPowers; i++)
            {
                createPower(position);
                position.x += spaceBetween;
                yield return Timing.WaitForSeconds(_settings.timeBetweenPowers);
            }

            yield return 0;
        }

        private void createPower(Vector2 position)
        {
            var power = GameObject.Instantiate(_settings.powerPrefab, position, Quaternion.identity) as GameObject;
        }
        #endregion

        [System.Serializable]
        public class Settings
        {
            public float timeToStartAttack;
            public float timeBetweenPowers;
            public float timeVulnerable;
            public int minPowerNumber;
            public int maxPowerNumber;

            public int numberOfPowers;
            public float powerAreaLeftPosition;
            public float powerAreaRightPosition;
            public float powerYPosition;

            public GameObject powerPrefab;
        }
    }
}