using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class EnemyController : MonoBehaviour, IStrikeable
{
    [SerializeField]
    private int _strikeDamage;
    [SerializeField]
    private float _timeBetweenAttacks;

    private ProximitySensor _sensor;
    private Attack _attack;
    private FaceDirection _faceDirection;
    private Life _life;
    private bool _isInsideAttackArea;

    void Start()
    {
        _sensor = GetComponent<ProximitySensor>();
        Assert.IsNotNull(_sensor);
        _attack = GetComponent<Attack>();
        Assert.IsNotNull(_attack);
        _faceDirection = GetComponent<FaceDirection>();
        Assert.IsNotNull(_faceDirection);
        _life = GetComponent<Life>();

        _sensor.AttackSensor.OnInsideSensor += onInsideAttackSensor;
        _sensor.ReadySensor.OnInsideSensor += onEnterInsideSensor;

        _attack.OnAttackHit += onAttackHit;

        if (_life != null)
            _life.OnDead += onDead;
    }

    private void onInsideAttackSensor(GameObject target)
    {
        if (!_isInsideAttackArea)
            StartCoroutine(doAttack());
    }

    private IEnumerator doAttack()
    {
        _isInsideAttackArea = true;

        if (enabled)
            _attack.DoAttack(_faceDirection.Direction);

        yield return new WaitForSeconds(_timeBetweenAttacks);
        _isInsideAttackArea = false;
    }

    private void onEnterInsideSensor(GameObject target)
    {
        var direction = (Vector2)(target.transform.position - transform.position).normalized;
        var directionX = (int)Mathf.Sign(direction.x);

        if (directionX != 0 && !_attack.enabled)
            _faceDirection.SetDirection(directionX);
    }

    private void onAttackHit(GameObject[] hits)
    {
        foreach (var hit in hits)
        {
            if (hit.tag == "Player")
            {
                var life = hit.GetComponent<Life>();

                life.TakeDamage(_strikeDamage, gameObject);
            }
        }
    }

    public void onDead()
    {
        enabled = false;
        var collider = GetComponent<Collider2D>();
        collider.enabled = false;
        _sensor.enabled = false;
    }

    public void Striked(int damage, Vector2 direction)
    {
        //_life.TakeDamage(damage, direction);
    }
}
