using Zenject;
using Gamelogic.Extensions;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using System;

namespace GG
{
    public class BossAI : IInitializable
    {
        [Inject]
        private BossFightStartSignal _bossFightStartSignal;
        [Inject]
        private LietoResurrectSignal _resurrectSignal;
        [Inject]
        private LietoDeathSignal _deathSignal;
        [Inject]
        private BossDeathSignal _bossDeathSignal;
        [Inject]
        private Life _life;

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
            _stateMachine.AddState(State.Waiting);
            _stateMachine.AddState(State.Idle, enterIdle, null, leaveIdle);
            _stateMachine.AddState(State.Attacking, enterAttack, null, leaveAttack);

            _stateMachine.CurrentState = State.Waiting;

            _bossFightStartSignal += bossFightStart;

            _resurrectSignal += onResurrect;
            _deathSignal += onDeath;

            _life.OnDead += onDead;

            UnityEngine.Random.InitState(9999);
        }

        private void onDead()
        {
            _stateMachine.CurrentState = State.Waiting;
            Timing.RunCoroutine(bossDeath());
        }

        private IEnumerator<float> bossDeath()
        {
            yield return Timing.WaitForSeconds(2);
            _bossDeathSignal.Fire();
        }

        private void bossFightStart()
        {
            _stateMachine.CurrentState = State.Attacking;
        }

        private void onResurrect()
        {
            _stateMachine.CurrentState = State.Attacking;
            _life.Reset();
        }

        private void onDeath()
        {
            _stateMachine.CurrentState = State.Waiting;
        }

        #region Idle State
        private void enterIdle()
        {
            Timing.RunCoroutine(startIdle(), "idle");
        }

        private void leaveIdle()
        {
            Timing.KillCoroutines("idle");
        }

        private IEnumerator<float> startIdle()
        {
            yield return Timing.WaitForSeconds(_settings.timeVulnerable);

            _stateMachine.CurrentState = State.Attacking;
        }
        #endregion

        #region Attack State
        private void enterAttack()
        {
            Timing.RunCoroutine(startAttack(), "attack");
            var fire = GameObject.FindObjectOfType<BossFire>();
            fire.LightOn();
        }

        private void leaveAttack()
        {
            Timing.KillCoroutines("attack");
            var fire = GameObject.FindObjectOfType<BossFire>();
            fire.LightOff();
        }

        private IEnumerator<float> startAttack()
        {
            yield return Timing.WaitForSeconds(_settings.timeToStartAttack);

            var spaceBetween = Mathf.Abs(_settings.powerAreaLeftPosition - _settings.powerAreaRightPosition) / (_settings.numberOfPowers - 1);

            var isRandom = UnityEngine.Random.Range(0, 100) <= _settings.randomChance;

            if (!isRandom)
            {
                var position = new Vector2(_settings.powerAreaLeftPosition, _settings.powerYPosition);

                for (int i = 0; i < _settings.numberOfPowers; i++)
                {
                    createPower(position);
                    position.x += spaceBetween;
                    yield return Timing.WaitForSeconds(_settings.timeBetweenPowers);
                }
            }
            else
            {
                var numberOfPowers = UnityEngine.Random.Range(_settings.minPowerNumber, _settings.maxPowerNumber + 1);

                var powerIndexes = new int[numberOfPowers];
                    var rnd = new System.Random();

                for (int i = 0; i < powerIndexes.Length; i++)
                {
                    var value = rnd.Next(1, _settings.numberOfPowers + 1);

                    while (getUniqueIndex(value, powerIndexes))
                    {
                        value = rnd.Next(1, _settings.numberOfPowers + 1);
                    }

                    powerIndexes[i] = value;
                }

                for (int i = 0; i < _settings.numberOfPowers; i++)
                {
                    if (contains(i + 1, powerIndexes))
                    {
                        var position = new Vector2(_settings.powerAreaLeftPosition + (spaceBetween * i), _settings.powerYPosition);

                        createPower(position);
                    }
                }
            }

            yield return Timing.WaitForSeconds(_settings.timeBetweenPowers * (_settings.numberOfPowers / 3));

            _stateMachine.CurrentState = State.Idle;
        }

        private bool getUniqueIndex(int value, int[] powerIndexes)
        {
            for (int l = 0; l < powerIndexes.Length; l++)
            {
                if (value == powerIndexes[l])
                    return true;
            }

            return false;
        }

        private bool contains(int value, int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (value == array[i])
                    return true;
            }

            return false;
        }

        private void createPower(Vector2 position)
        {
            var power = GameObject.Instantiate(_settings.powerPrefab, position, Quaternion.identity) as GameObject;
            power.GetComponent<BossPower>().timeToExplode = _settings.timeToExplode;
        }
        #endregion

        [System.Serializable]
        public class Settings
        {
            public float timeToStartAttack;
            public float timeBetweenPowers;
            public float timeVulnerable;
            public float timeToExplode;
            public int minPowerNumber;
            public int maxPowerNumber;
            public float randomChance;
            public int numberOfPowers;
            public float powerAreaLeftPosition;
            public float powerAreaRightPosition;
            public float powerYPosition;

            public GameObject powerPrefab;
        }
    }
}