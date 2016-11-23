using System;
using Zenject;

namespace GG
{
    public class LietoDeathSignal : Signal<LietoDeathSignal> { }

    public class LietoDeath : IInitializable
    {
        private FaceDirection _faceDirection;
        private Attack _attack;
        private Jump _jump;
        private Move _move;
        private Life _life;
        private LietoPushed _pushed;
        private LietoDeathSignal _deathSignal;
        private Switch _switch;

        public LietoDeath(FaceDirection faceDirection, Attack attack, Jump jump,
            Move move, Life life, LietoPushed pushed,
            LietoDeathSignal deathSignal,
            DetectCheckpoint detectCheckpoint, CharacterMotor motor,
            CameraFollow follow, Switch mySwitch)
        {
            _faceDirection = faceDirection;
            _attack = attack;
            _jump = jump;
            _move = move;
            _life = life;
            _pushed = pushed;
            _deathSignal = deathSignal;
            _switch = mySwitch;
        }

        public void Initialize()
        {
            _life.OnDead += OnDead;
            _life.OnResurrect += OnRessurect;
        }

        public void OnDead()
        {
            setEnables(false);
            _deathSignal.Fire();
        }

        public void OnRessurect()
        {
            setEnables(true);
            _faceDirection.Reset();
        }

        private void setEnables(bool value)
        {
            _faceDirection.SetEnable(value);
            _attack.SetEnable(value);
            _jump.SetEnable(value);
            _move.SetEnable(value);
            _pushed.SetEnable(value);
            _switch.SetEnable(value);
        }

    }
}