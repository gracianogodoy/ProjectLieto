namespace GG
{
    public class LietoDeath
    {
        private FaceDirection _faceDirection;
        private Attack _attack;
        private Jump _jump;
        private Move _move;

        public LietoDeath(FaceDirection faceDirection, Attack attack, Jump jump, Move move)
        {
            _faceDirection = faceDirection;
            _attack = attack;
            _jump = jump;
            _move = move;
        }

        public void OnDead()
        {
            setEnables(false);
        }

        public void Ressurect()
        {
            setEnables(true);
        }

        private void setEnables(bool value)
        {
            _faceDirection.SetEnable(value);
            _attack.SetEnable(value);
            _jump.SetEnable(value);
            _move.SetEnable(value);
        }
    }
}