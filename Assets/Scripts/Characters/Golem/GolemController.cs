using UnityEngine;
using UnityEngine.Assertions;

public class GolemController : MonoBehaviour, IStrikeable
{
    [SerializeField]
    private int _strikeDamage;

    private ProximitySensor _sensor;
    private Attack _attack;
    private FaceDirection _faceDirection;
    private Life _life;

    void Start()
    {
        _sensor = GetComponent<ProximitySensor>();
        Assert.IsNotNull(_sensor);
        _attack = GetComponent<Attack>();
        Assert.IsNotNull(_attack);
        _faceDirection = GetComponent<FaceDirection>();
        Assert.IsNotNull(_faceDirection);
        _life = GetComponent<Life>();
        Assert.IsNotNull(_life);

        _sensor.AttackSensor.OnInsideSensor += onInsideAttackSensor;
        _sensor.ReadySensor.OnInsideSensor += onEnterInsideSensor;

        _attack.OnAttackHit += onAttackHit;
    }

    private void onInsideAttackSensor(GameObject target)
    {
        if (!_attack.enabled)
            _attack.DoAttack(_faceDirection.Direction);
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

    public void Striked(int damage, GameObject other)
    {
        _life.TakeDamage(damage);
    }
}
