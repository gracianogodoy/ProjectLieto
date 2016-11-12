using System;
using Zenject;

namespace GG
{
    public class LietoPushed : IInitializable
    {
        private Pushback _pushback;
        private Move _move;
        private Attack _attack;
        private Jump _jump;

        public LietoPushed(Pushback pushback, Move move, Attack attack, Jump jump)
        {
            _pushback = pushback;
            _move = move;
            _attack = attack;
            _jump = jump;
        }

        public void Initialize()
        {
            _pushback.OnPushbackStart += () => setEnable(false);
            _pushback.OnPushbackEnd += () => setEnable(true);
        }

        private void setEnable(bool value)
        {
            _move.SetEnable(value);
            _jump.SetEnable(value);
            _attack.SetEnable(value);
        }
    }
}
