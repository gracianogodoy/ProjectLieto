using System.Collections;
using UnityEngine;
using UniRx;

public class GhoulController : MonoBehaviour
{
    [SerializeField]
    private float _chaseSpeed;
    [SerializeField]
    private float _patrolSpeed;
    [SerializeField]
    private float _distanceToStop;

    private ProximitySensor _sensor;
    private CharacterMotor _motor;
    private FaceDirection _faceDirection;
    private Vector2 _initialPosition;

    private bool _isPatrolling;
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


        _direction = Vector2.right;
        _isPatrolling = true;
        MainThreadDispatcher.StartUpdateMicroCoroutine(patrolling());

        var parent = GameObject.Find("Characters2");

        transform.parent = parent.transform;
        _initialPosition = transform.localPosition;

    }

    private void onInsideSensor(GameObject target)
    {
        var _direction = target.transform.position - transform.localPosition;

        _faceDirection.SetDirection((int)Mathf.Sign(_direction.x));

        if (Vector2.Distance(transform.localPosition, target.transform.position) <= _distanceToStop)
        {
            _motor.Velocity = Vector2.zero;
        }
        else
        {
            _motor.Velocity = _direction.normalized * _chaseSpeed;
        }
    }

    private void onLeaveSensor(GameObject target)
    {
        _motor.Velocity = Vector2.zero;
        _isPatrolling = true;
        MainThreadDispatcher.StartUpdateMicroCoroutine(backToInitialPosition());
    }

    private void onEnterSensor(GameObject target)
    {
        _isPatrolling = false;
        _isChasing = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ChangeGhoulDirection" && !_isChasing)
        {
            _direction *= -1;
        }
    }

    private IEnumerator backToInitialPosition()
    {
        while (Vector2.Distance(transform.localPosition, _initialPosition) >= 0.1f && _isPatrolling)
        {
            var direction = _initialPosition - (Vector2)transform.localPosition;
            _faceDirection.SetDirection((int)Mathf.Sign(direction.x));

            transform.localPosition = Vector2.MoveTowards(transform.localPosition, _initialPosition, _patrolSpeed * Time.deltaTime);

            yield return null;
        }

        MainThreadDispatcher.StartUpdateMicroCoroutine(patrolling());
        _isChasing = false;
    }

    private IEnumerator patrolling()
    {
        while (_isPatrolling)
        {
            _faceDirection.SetDirection((int)Mathf.Sign(_direction.x));

            transform.localPosition += (Vector3)_direction * Time.deltaTime * _patrolSpeed;

            yield return null;
        }
    }
}
