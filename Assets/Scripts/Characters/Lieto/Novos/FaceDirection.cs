using System;
using Zenject;

namespace GG
{
    public class FaceDirection : BaseCharacterBehaviour, IInitializable
    {
        public enum Facing
        {
            Right = 1,
            Left = -1
        }

        private Settings _settings;

        private int _direction = 1;

        public int Direction { get { return _direction; } }

        public Action<int> OnChangeDirection { get; set; }

        public FaceDirection(Settings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            SetDirection((int)_settings.InitialDirection);
        }

        public void SetDirection(int direction)
        {
            if (!_isEnable)
                return;

            if (direction != 0)
                _direction = direction;

            if (OnChangeDirection != null)
                OnChangeDirection(_direction);
        }

        [Serializable]
        public class Settings
        {
            public Facing InitialDirection = Facing.Right;
        }
    }
}