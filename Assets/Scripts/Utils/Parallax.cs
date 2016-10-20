using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float SpeedX;
    public float SpeedY;
    public bool InvertDirection;

    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
    }

    void Update()
    {
        move();
    }

    private void move()
    {
        var cameraDeltaMovement = _cameraTransform.position - _lastCameraPosition;

        var velocity = Vector3.Scale(cameraDeltaMovement, new Vector3(SpeedX, SpeedY, 0));

        velocity = InvertDirection ? -velocity : velocity;

        transform.position += velocity;

        _lastCameraPosition = _cameraTransform.position;
    }
}
