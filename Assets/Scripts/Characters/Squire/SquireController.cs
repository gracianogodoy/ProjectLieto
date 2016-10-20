﻿using System.Collections;
using UnityEngine;

public class SquireController : MonoBehaviour
{
    [SerializeField]
    private float _idleTime;

    [SerializeField]
    private float _maxMoveDistance;
    [SerializeField]
    private float _minMoveDistance;

    private bool _isWalking;
    private float _moveDistance;
    private float _moveCurrentDistance;
    private float _lastPositionX;

    private Move _move;
    private FaceDirection _faceDirection;
    private CharacterMotor _motor;

    void Start()
    {
        _move = GetComponent<Move>();
        _faceDirection = GetComponent<FaceDirection>();
        _motor = GetComponent<CharacterMotor>();

        _motor.OnTriggerEnter += onTriggerEnter;

        StartCoroutine(idle());
    }

    void Update()
    {
        if (_isWalking)
            walking();
    }

    private IEnumerator idle()
    {
        yield return new WaitForSeconds(_idleTime);
        startWalk();
    }

    private void startWalk()
    {
        _moveDistance = Random.Range(_minMoveDistance, _maxMoveDistance);
        _lastPositionX = transform.position.x;
        _moveCurrentDistance = 0;
        _isWalking = true;
    }

    private void walking()
    {
        _moveCurrentDistance += Mathf.Abs(transform.position.x - _lastPositionX);
        _lastPositionX = transform.position.x;

        _move.DoMove(_faceDirection.Direction);

        if (_moveCurrentDistance > _moveDistance)
        {
            _isWalking = false;
            _move.DoMove(0);
            randomizeFaceDirection();
            StartCoroutine(idle());
        }
    }

    private void randomizeFaceDirection()
    {
        var direction = Random.Range(0, 1.0f) < 0.5f ? -1 : 1;
        _faceDirection.SetDirection(direction);
    }

    private void onTriggerEnter(Collider2D other)
    {
        _faceDirection.SetDirection(-_faceDirection.Direction);
    }
}