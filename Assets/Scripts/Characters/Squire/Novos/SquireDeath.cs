using Zenject;

namespace GG
{
    public class SquireDeath : IInitializable
    {
        private FaceDirection _faceDirection;
        private Move _move;
        private SquireAI _squireAi;
        private Life _life;

        public SquireDeath(FaceDirection faceDirection, Move move, SquireAI squireAi, Life life)
        {
            _faceDirection = faceDirection;
            _move = move;
            _squireAi = squireAi;
            _life = life;
        }

        public void Initialize()
        {
            _life.OnDead += OnDead;
        }

        public void OnDead()
        {
            setEnables(false);
        }

        private void setEnables(bool value)
        {
            _faceDirection.SetEnable(value);
            _move.SetEnable(value);

            _squireAi.SetState(SquireAI.State.Dead);
        }
    }
}