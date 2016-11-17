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

        public BaseResurrect(LietoResurrectSignal resurrectSignal, CharacterMotor motor)
        {
            _resurrectSignal = resurrectSignal;
            _motor = motor;
        }

        public virtual void Initialize()
        {
            _resurrectSignal += resurrect;

            _ressurectPosition = _motor.Position;
        }

        public void Dispose()
        {
            _resurrectSignal -= resurrect;
        }

        private void resurrect()
        {
            _motor.Position = _ressurectPosition;
            onResurrect();
        }

        protected abstract void onResurrect();
    }
}