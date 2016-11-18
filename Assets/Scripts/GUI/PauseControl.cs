using UnityEngine;
using Zenject;
using Gamelogic.Extensions;
using System;

namespace GG
{
    public class PauseSignal : Signal<PauseSignal> { }
    public class UnpauseSignal : Signal<UnpauseSignal> { }

    public class PauseControl : IInitializable, ITickable
    {
        private PauseSignal _pauseSignal;
        private UnpauseSignal _unpauseSignal;

        public enum State
        {
            Paused,
            Unpaused,
        }

        private bool _isPaused;

        private StateMachine<State> _stateMachine = new StateMachine<State>();

        public PauseControl(PauseSignal pauseSignal, UnpauseSignal unpauseSignal)
        {
            _pauseSignal = pauseSignal;
            _unpauseSignal = unpauseSignal;
        }

        public void Initialize()
        {
            _stateMachine.AddState(State.Paused, enterPaused);
            _stateMachine.AddState(State.Unpaused, enterUnpaused);

            _stateMachine.CurrentState = State.Unpaused;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isPaused = !_isPaused;

                _stateMachine.CurrentState = _isPaused ? State.Paused : State.Unpaused;
            }
        }

        private void enterPaused()
        {
            Debug.Log("enter pause");
            Time.timeScale = 0;
            _pauseSignal.Fire();
        }

        private void enterUnpaused()
        {
            Time.timeScale = 1;
            _unpauseSignal.Fire();
        }
    }
}