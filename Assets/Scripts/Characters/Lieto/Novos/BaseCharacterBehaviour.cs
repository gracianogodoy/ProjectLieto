namespace GG
{
    public class BaseCharacterBehaviour
    {
        protected CharacterMotor _motor;

        public BaseCharacterBehaviour(CharacterMotor motor)
        {
            _motor = motor;
        }

        protected bool _isEnable = true;

        public void SetEnable(bool enable)
        {
            _isEnable = enable;
        }
    }
}