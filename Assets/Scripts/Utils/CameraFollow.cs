using UnityEngine;

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
        //var VerticalHightSeen = _camera.orthographicSize * 2f;
        //var HorizontalHeightSeen = VerticalHightSeen * Screen.width / Screen.height;

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
        {
            followTarget();
        }
    }

    public void LockOnTarget()
    {
        SetPosition(_target.position.x, _target.position.y);
    }

    public void SetPosition(Vector2 position)
    {
        var posX = position.x;
        var posY = position.y;

        posX = Mathf.Clamp(posX, MinCameraPosition.x, MaxCameraPosition.x);
        posY = Mathf.Clamp(posY, MinCameraPosition.y, MaxCameraPosition.y);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }

    public void SetPosition(float x, float y)
    {
        SetPosition(new Vector2(x, y));
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void followTarget()
    {
        var posX = Mathf.Lerp(transform.position.x, _target.position.x, Speed * Time.deltaTime);
        var posY = Mathf.Lerp(transform.position.y, _target.position.y, Speed * Time.deltaTime);

        SetPosition(posX, posY);
    }
}
