using System;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class Move : MonoBehaviour
{
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _acceleration;

    private CharacterMotor _motor;
    private int _direction;

    public int Direction
    {
        get
        {
            return _direction;
        }
    }

    void Start()
    {
        _motor = GetComponent<CharacterMotor>();
    }

    void Update()
    {
        var velocity = _motor.Velocity.x;

        velocity = Mathf.MoveTowards(velocity, _maxSpeed * Direction, _acceleration * Time.deltaTime);

        _motor.Velocity = new Vector2(velocity, _motor.Velocity.y);

        if (Mathf.Abs(_motor.Velocity.x) <= 0)
        {
            enabled = false;
        }
    }

    public void DoMove(int direction)
    {
        enabled = true;
        _direction = direction;
    }

    public void StopMove()
    {
        _direction = 0;
    }
}
