using UnityEngine;

public class GhoulController : MonoBehaviour
{
    [SerializeField]
    private float _chaseVelocity;
    [SerializeField]
    private float _distanceToStop;

    private ProximitySensor _sensor;
    private CharacterMotor _motor;
    private FaceDirection _faceDirection;

    private bool _isChasing;
    private Vector2 _direction;

    void Start()
    {
        _motor = GetComponent<CharacterMotor>();
        _faceDirection = GetComponent<FaceDirection>();

        _sensor = GetComponent<ProximitySensor>();
        _sensor.ReadySensor.OnEnterSensor += onEnterSensor;
        _sensor.ReadySensor.OnLeaveSensor += onLeaveSensor;
        _sensor.ReadySensor.OnInsideSensor += onInsideSensor;
    }

    private void onInsideSensor(GameObject target)
    {
        _direction = target.transform.position - transform.position;

        _faceDirection.SetDirection((int)Mathf.Sign(_direction.x));

        if (Vector2.Distance(transform.position, target.transform.position) <= _distanceToStop)
        {
            _motor.Velocity = Vector2.zero;
        }
        else
        {
            _motor.Velocity = _direction.normalized * _chaseVelocity;
        }
    }

    private void onLeaveSensor(GameObject target)
    {
        _motor.Velocity = Vector2.zero;
        _isChasing = false;
    }

    private void onEnterSensor(GameObject target)
    {
        _isChasing = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "ChangeGhoulDirection")
            return;


    }
}
