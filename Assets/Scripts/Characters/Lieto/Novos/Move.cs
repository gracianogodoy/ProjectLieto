namespace GG
{
    public class Move : BaseCharacterBehaviour
    {
        private Settings _settings;
        private CharacterMotor _motor;

        public Move(CharacterMotor motor, Settings settings)
        {
            _settings = settings;
            _motor = motor;
        }

        public void OnMove(int direction)
        {
            if (_isEnable)
            {
                _motor.SetVelocityX(_settings.speed * direction);
            }
        }

        [System.Serializable]
        public class Settings
        {
            public float speed;
        }
    }
}
