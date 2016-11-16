using UnityEngine;
using Zenject;
using System;

namespace GG
{
    public class LietoRessurectSignal : Signal<LietoRessurectSignal> { }

    public class LietoResurrect : IInitializable, IDisposable
    {
        private LietoRessurectSignal _ressurectSignal;
        private DetectCheckpoint _detectCheckpoint;
        private CharacterMotor _motor;
        private CameraFollow _cameraFollow;
        private Life _life;
        private Switch _switch;

        private Vector2 _ressurectPosition;

        public LietoResurrect(DetectCheckpoint detectCheckpoint, CharacterMotor motor,
            CameraFollow follow, LietoRessurectSignal ressurectSignal, Life life, Switch _switch)
        {
            _detectCheckpoint = detectCheckpoint;
            _cameraFollow = follow;
            _motor = motor;
            _ressurectSignal = ressurectSignal;
            _life = life;
            this._switch = _switch;
        }

        public void Initialize()
        {
            _ressurectSignal += OnRessurect;

            _ressurectPosition = _motor.Position;
            _detectCheckpoint.OnDetect += (other) =>
            {
                _ressurectPosition = other.transform.localPosition;
            };
        }

        public void Dispose()
        {
            _ressurectSignal -= OnRessurect;
        }

        public void OnRessurect()
        {
            _switch.SwtichToWorld1();
            _motor.Position = _ressurectPosition;
            _cameraFollow.LockOnTarget();
            _life.Ressurect();
        }
    }
}