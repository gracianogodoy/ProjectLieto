using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour
{
    private Transform _lastCheckpoint;
    private Vector2 _initialPosition;

    public Vector2 LastCheckpoint
    {
        get
        {
            if (_lastCheckpoint != null)
                return _lastCheckpoint.position;
            else
                return _initialPosition;
        }
    }

    void Start()
    {
        _initialPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint")
        {
            _lastCheckpoint = other.transform;
        }
    }
}
