namespace GG
{
    public class BaseCharacterBehaviour
    {
        protected bool _isEnable = true;

        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
        }

        public void SetEnable(bool enable)
        {
            _isEnable = enable;
        }
    }
}