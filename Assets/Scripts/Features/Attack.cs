using UnityEngine;
using Zenject;

namespace GG
{
    public class Attack : BaseCharacterBehaviour, ITickable
    {
        private Settings _settings;
        private FaceDirection _faceDirection;
        private GameObject _owner;

        private float _elapsedTime;
        private bool _attacked;
        private bool _isAttacking;
        private int _direction;

        public System.Action<GameObject[]> OnAttackHit { get; set; }
        public System.Action OnAttackMiss { get; set; }
        public System.Action OnAttack { get; set; }

        public bool IsAttacking
        {
            get
            {
                return _isAttacking;
            }
        }

        public FaceDirection FaceDirection
        {
            get
            {
                return _faceDirection;
            }
        }

        public Attack(Settings settings, FaceDirection faceDirection, [Inject(Id = InjectId.Owner)] GameObject owner)
        {
            _settings = settings;
            _owner = owner;
            _faceDirection = faceDirection;
        }

        public void DoAttack()
        {
            if (!_isAttacking && _isEnable)
            {
                _isAttacking = true;
                _direction = _faceDirection.Direction;
                if (OnAttack != null)
                    OnAttack();
            }
        }

        public void Reset()
        {
            _elapsedTime = 0;
            _isAttacking = false;
            _attacked = false;
            _isEnable = true;
        }

        public void Tick()
        {
            if (!_isEnable || !_isAttacking)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _settings.TimeToAttack && !_attacked)
            {
                castRay(_direction, _owner);
                _attacked = true;
            }

            if (_elapsedTime >= _settings.Duration)
            {
                _isAttacking = false;
                _attacked = false;
                _elapsedTime = 0;
            }
        }
        private void castRay(int direction, GameObject gameObject)
        {
            var position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + _settings.Offset);
            var hits = gameObject.RaycastMany(position, new Vector2(direction, 0), _settings.Distance,
                _settings.HitLayer, _settings.BaseSize, _settings.NumberOfRays);

            if (hits.Length > 0)
            {
                if (OnAttackHit != null)
                    OnAttackHit(hits);
            }
            else
            {
                if (OnAttackMiss != null)
                    OnAttackMiss();
            }
        }

        [System.Serializable]
        public class Settings
        {
            public float Duration;
            public float TimeToAttack;
            public float Distance;
            public float BaseSize;
            public int NumberOfRays;
            public float Offset;
            public LayerMask HitLayer;
        }
    }
}