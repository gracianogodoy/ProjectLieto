﻿using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string TargetName;
    public float Speed;

    public Vector2 MinCameraPosition;
    public Vector2 MaxCameraPosition;

    private Transform _target;

    void Start()
    {
        var targetGameObject = GameObject.Find(TargetName);

        if (targetGameObject != null)
            _target = targetGameObject.transform;
    }

    void Update()
    {
        if (_target == null)
        {
            var targetGameObject = GameObject.Find(TargetName);

            if (targetGameObject != null)
                _target = targetGameObject.transform;
        }

        if (_target != null)
            followTarget();
    }

    private void followTarget()
    {
        var posX = Mathf.Lerp(transform.position.x, _target.position.x, Speed * Time.deltaTime);
        var posY = Mathf.Lerp(transform.position.y, _target.position.y, Speed * Time.deltaTime);

        posX = Mathf.Clamp(posX, MinCameraPosition.x, MaxCameraPosition.x);
        posY = Mathf.Clamp(posY, MinCameraPosition.y, MaxCameraPosition.y);

        transform.position = new Vector3( posX, posY, transform.position.z);
    }
}
