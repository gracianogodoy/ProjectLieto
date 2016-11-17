using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GG
{
    public class Switch
    {
        private SwitchWorld _switchWorld;
        private CharacterMotor _motor;
        private Life _life;
        private Jump _jump;
        private Settings _settings;

        private Collider2D _collider;
        private bool _isOnBadWorld;
        private bool _isTouching;

        public bool IsOnBadWorld
        {
            get
            {
                return _isOnBadWorld;
            }
        }

        public Switch(SwitchWorld switchWorld, CharacterMotor motor, Life life, Jump jump,
            Settings settings, [Inject(Id = InjectId.Owner)] GameObject owner)
        {
            _switchWorld = switchWorld;
            _life = life;
            _motor = motor;
            _jump = jump;
            _settings = settings;
            _collider = owner.GetComponent<Collider2D>();
        }

        public void OnSwitch()
        {
            _switchWorld.Switch();
            _isOnBadWorld = !_isOnBadWorld;
            checkForCollider();
        }

        public void SwtichToWorld1()
        {
            _switchWorld.SwitchTo("world1", true);
        }

        private void checkForCollider()
        {
            var offset = 0.4f;
            var myBounds = _collider.bounds;
            myBounds.size = new Vector3(myBounds.size.x - offset, myBounds.size.y - offset);

            var hit = Physics2D.BoxCast(myBounds.center, myBounds.size, 0, Vector2.zero, 0, _settings.obstacleMask);

            if (hit)
            {
                _motor.SetVelocity(Vector2.zero);
                _isTouching = true;
                Timing.RunCoroutine(waitToSwitchBack(_settings.timeToSwitchBackAfterDamage));
            }
        }

        private IEnumerator<float> waitToSwitchBack(float time)
        {
            yield return Timing.WaitForSeconds(time);

            if (_isTouching)
            {
                _switchWorld.Switch();
                _isOnBadWorld = !_isOnBadWorld;
                _life.TakeDamage(_settings.damageOnOverlapping);
                _isTouching = false;
                //_motor.ToggleGravity(true);
            }

            yield return Timing.WaitForSeconds(0.1f);
            //_motor.ToggleGravity(true);
        }

        [System.Serializable]
        public class Settings
        {
            public LayerMask obstacleMask;
            public int damageOnOverlapping;
            public float timeToSwitchBackAfterDamage;
        }
    }
}