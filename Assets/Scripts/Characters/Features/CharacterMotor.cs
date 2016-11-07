using UnityEngine;
using Prime31;
using System;
using UnityEngine.Assertions;

public class CharacterMotor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Será afetado pelo delta time.")]
    private float _gravity;
    [SerializeField]
    private float _maxFallSpeed;
    [SerializeField]
    private CharacterController2D _controller;
    [SerializeField]
    private bool _hasGravity = true;

    private Vector2 _velocity;

    public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }
    public CharacterController2D.CharacterCollisionState2D CollisionState { get { return _controller.collisionState; } }
    public Action<Collider2D> OnTriggerEnter { get; set; }

    public bool IsGrounded { get { return _controller.isGrounded; } }

    void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        Assert.IsNotNull(_controller);
    }

    void Update()
    {
        applyGravity();

        _controller.move(_velocity * Time.deltaTime);
    }

    private void applyGravity()
    {
        if (!_hasGravity)
            return;

        if (!_controller.isGrounded)
            _velocity.y -= _gravity * Time.deltaTime;

        _velocity.y = Mathf.Max(_velocity.y, -_maxFallSpeed);
    }

    public void ToggleGravity(bool enable)
    {
        _hasGravity = enable;
    }
}

