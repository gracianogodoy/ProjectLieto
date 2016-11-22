using System;
using UnityEngine;
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
        private Collider2D _collider;

        public SquireDeath(FaceDirection faceDirection, Move move, SquireAI squireAi, Life life, SquireDeathSignal deathSignal, 
            [Inject(Id = InjectId.Owner)] GameObject owner)
        {
            _faceDirection = faceDirection;
            _move = move;
            _squireAi = squireAi;
            _life = life;
            _deathSignal = deathSignal;
            _collider = owner.GetComponent<Collider2D>();
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
            _collider.enabled = value;
        }
    }
}