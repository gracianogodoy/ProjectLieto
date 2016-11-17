using Zenject;
using System;

namespace GG
{
    public class LietoResurrectSignal : Signal<LietoResurrectSignal> { }

    public class LietoResurrect : BaseResurrect, IInitializable, IDisposable
    {
        private DetectCheckpoint _detectCheckpoint;
        private CameraFollow _cameraFollow;
        private Life _life;
        private Switch _switch;

        public LietoResurrect(DetectCheckpoint detectCheckpoint, CharacterMotor motor,
            CameraFollow follow, LietoResurrectSignal resurrectSignal, Life life, Switch _switch) : base(resurrectSignal, motor)
        {
            _detectCheckpoint = detectCheckpoint;
            _cameraFollow = follow;
            _life = life;
            this._switch = _switch;
        }

        public override void Initialize()
        {
            base.Initialize();

            _detectCheckpoint.OnDetect += (other) =>
            {
                _ressurectPosition = other.transform.localPosition;
            };
        }

        protected override void onResurrect()
        {
            _switch.SwtichToWorld1();
            _cameraFollow.LockOnTarget();
            _life.Ressurect();
        }
    }
}