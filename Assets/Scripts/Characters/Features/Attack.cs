using UnityEngine;

public class Attack : MonoBehaviour
{
    public delegate void AttackHitEvent(GameObject[] hits);
    public delegate void AttackMissEvent();

    [SerializeField]
    private Settings _settings;

    private float _elapsedTime;
    private bool _attacked;
    private int _direction;

    public AttackHitEvent OnAttackHit { get; set; }
    public AttackMissEvent OnAttackMiss { get; set; }

    public void DoAttack(int direction)
    {
        if (!enabled)
        {
            enabled = true;
            _direction = direction;
        }
    }

    public void Reset()
    {
        _elapsedTime = 0;
        enabled = false;
        _attacked = false;
    }

    public void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _settings.TimeToAttack && !_attacked)
        {
            castRay(_direction);
            _attacked = true;
        }

        if (_elapsedTime >= _settings.Duration)
        {
            enabled = false;
            _attacked = false;
            _elapsedTime = 0;
        }
    }

    private void castRay(int direction)
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
