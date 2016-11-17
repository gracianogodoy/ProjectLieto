using UnityEngine;
using Zenject;
using System;

namespace GG
{
    public abstract class BaseResurrect : IInitializable, IDisposable
    {
        protected LietoResurrectSignal _resurrectSignal;
        protected CharacterMotor _motor;
        protected Vector2 _ressurectPosition;
        protected Life _life;

        public BaseResurrect(LietoResurrectSignal resurrectSignal, CharacterMotor motor, Life life)
        {
            _resurrectSignal = resurrectSignal;
            _motor = motor;
            _life = life;
        }

        public virtual void Initialize()
        {
            _resurrectSignal += resurrect;

            _ressurectPosition = _motor.LocalPosition;
        }

        public void Dispose()
        {
            _resurrectSignal -= resurrect;
        }

        private void resurrect()
        {
            _motor.LocalPosition = _ressurectPosition;
            onResurrect();
            _life.Ressurect();
        }

        protected abstract void onResurrect();
    }
}