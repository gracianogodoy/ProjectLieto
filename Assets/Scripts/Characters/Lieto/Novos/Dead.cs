using Zenject;

namespace GG
{
    public class LietoDeath : IInitializable
    {
        private FaceDirection _faceDirection;
        private Attack _attack;
        private Jump _jump;
        private Move _move;
        private Life _life;
        private LietoPushed _pushed;

        public LietoDeath(FaceDirection faceDirection, Attack attack, Jump jump, Move move, Life life, LietoPushed pushed)
        {
            _faceDirection = faceDirection;
            _attack = attack;
            _jump = jump;
            _move = move;
            _life = life;
            _pushed = pushed;
        }

        public void Initialize()
        {
            _life.OnDead += OnDead;
            _life.OnRessurect += OnRessurect;
        }

        public void OnDead()
        {
            setEnables(false);
        }

        public void OnRessurect()
        {
            setEnables(true);
        }

        private void setEnables(bool value)
        {
            _faceDirection.SetEnable(value);
            _attack.SetEnable(value);
            _jump.SetEnable(value);
            _move.SetEnable(value);
            _pushed.SetEnable(value);
        }
    }
}