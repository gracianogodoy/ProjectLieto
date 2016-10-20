using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class Jump : MonoBehaviour
{
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private float _shortJumpPower;

    private CharacterMotor _motor;

    void Start()
    {
        _motor = GetComponent<CharacterMotor>();
    }

    void Update()
    {
        if (_motor.CollisionState.above)
        {
            changeVelocityY(0);
            enabled = false;
        }

        if (_motor.IsGrounded)
            enabled = false;
    }

    public void DoJump()
    {
        if (_motor.IsGrounded)
        {
            enabled = true;
            changeVelocityY(_jumpPower);
        }
    }

    public void StopJump()
    {
        if (!_motor.IsGrounded && _motor.Velocity.y > _shortJumpPower)
        {
            changeVelocityY(_shortJumpPower);
        }
    }

    public void changeVelocityY(float y)
    {
        _motor.Velocity = new Vector2(_motor.Velocity.x, y);
    }
}
