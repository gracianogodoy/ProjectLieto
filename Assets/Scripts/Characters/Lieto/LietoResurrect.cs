using Zenject;
using System;

namespace GG
{
    public class LietoResurrectSignal : Signal<LietoResurrectSignal> { }

    public class LietoResurrect : BaseResurrect, IInitializable, IDisposable
    {
        private DetectCheckpoint _detectCheckpoint;
        private CameraFollow _cameraFollow;
        private Switch _switch;
        private BossFightStartSignal _bossStartSignal;

        public LietoResurrect(DetectCheckpoint detectCheckpoint, CharacterMotor motor,
            CameraFollow follow, LietoResurrectSignal resurrectSignal, Life life, Switch _switch, BossFightStartSignal bossStartSignal) : base(resurrectSignal, motor, life)
        {
            _detectCheckpoint = detectCheckpoint;
            _cameraFollow = follow;
            _life = life;
            this._switch = _switch;
            _bossStartSignal = bossStartSignal;
        }

        public override void Initialize()
        {
            base.Initialize();

            _detectCheckpoint.OnDetect += (other) =>
            {
                _ressurectPosition = other.transform.localPosition;
            };

            _bossStartSignal += () =>
            {
                _ressurectPosition = _motor.Position;
            };
        }

        protected override void onResurrect()
        {
            _switch.SwtichToWorld1();
            _cameraFollow.LockOnTarget();
        }
    }
}