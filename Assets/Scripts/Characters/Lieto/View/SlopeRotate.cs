using UnityEngine;
using System.Collections;

public class SlopeRotate : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed;
    [SerializeField]
    private float _viewPositionOffset;

    private CharacterMotor _motor;
    private Vector3 _angle;

    void Start()
    {
        _motor = GetComponentInParent<CharacterMotor>();
    }

    void Update()
    {
        var targetAngle = _motor.CollisionState.slopeAngle;

        var targetAngleVector = new Vector3(0, 0, targetAngle);

        _angle = Vector3.MoveTowards(_angle, targetAngleVector, _rotateSpeed * Time.deltaTime);

        transform.localEulerAngles = _angle;

        if (targetAngle != 0)
        {
           transform.localPosition = new Vector2(0, _viewPositionOffset);
        }
        else
        {
            transform.localPosition = Vector2.zero;
        }
    }
}
