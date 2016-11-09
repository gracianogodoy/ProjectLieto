namespace GG
{
    public class Move : BaseCharacterBehaviour
    {
        private Settings _settings;

        public Move(CharacterMotor motor, Settings settings) : base(motor)
        {
            _settings = settings;
        }

        public void OnMove(int direction)
        {
            if (_isEnable)
                _motor.SetVelocityX(_settings.speed * direction);
        }

        [System.Serializable]
        public class Settings
        {
            public float speed;
        }
    }
}
