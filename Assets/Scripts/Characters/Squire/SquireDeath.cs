using System;
using Zenject;

namespace GG
{
    public class SquireDeath : IInitializable
    {
        private FaceDirection _faceDirection;
        private Move _move;
        private SquireAI _squireAi;
        private Life _life;
        private SquireDeathSignal _deathSignal;

        public SquireDeath(FaceDirection faceDirection, Move move, SquireAI squireAi, Life life, SquireDeathSignal deathSignal)
        {
            _faceDirection = faceDirection;
            _move = move;
            _squireAi = squireAi;
            _life = life;
            _deathSignal = deathSignal;
        }

        public void Initialize()
        {
            _life.OnDead += OnDead;
            _life.OnResurrect += OnRessurect;
        }

        private void OnRessurect()
        {
            setEnables(true);
            _faceDirection.Reset();
        }

        public void OnDead()
        {
            setEnables(false);
            _deathSignal.Fire();
            _squireAi.SetState(SquireAI.State.Dead);
        }

        private void setEnables(bool value)
        {
            _faceDirection.SetEnable(value);
            _move.SetEnable(value);
        }
    }
}