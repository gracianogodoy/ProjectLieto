using UnityEngine;
using Zenject;

public class FlipView : MonoBehaviour
{
    private GG.FaceDirection _faceDirection;

    [Inject]
    public void Construct(GG.FaceDirection faceDirection)
    {
        _faceDirection = faceDirection;
    }

    void OnValidate()
    {
        if (_faceDirection != null)
        {
            _faceDirection.OnChangeDirection -= onChangeDirection;
            _faceDirection.OnChangeDirection += onChangeDirection;
        }
    }

    void Start()
    {
        if (_faceDirection != null)
        {
            _faceDirection.OnChangeDirection -= onChangeDirection;
            _faceDirection.OnChangeDirection += onChangeDirection;
        }
    }

    private void onChangeDirection(int direction)
    {
        if (direction > 0)
            setLocalScaleX(true);
        if (direction < 0)
            setLocalScaleX(false);
    }

    private void setLocalScaleX(bool right)
    {
        var scaleX = right ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x);

        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
    }
}
