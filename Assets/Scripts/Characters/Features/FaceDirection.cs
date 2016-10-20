using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    public delegate void ChangeDirectionEvent(int direction);

    public enum Facing
    {
        Right = 1,
        Left = -1
    }

    [SerializeField]
    private Facing _initialDirection = Facing.Right;

    private int _direction = 1;

    public int Direction { get { return _direction; } }

    public ChangeDirectionEvent OnChangeDirection { get; set; }

    void OnValidate()
    {
        SetDirection((int)_initialDirection);
    }

    public void SetDirection(int direction)
    {
        _direction = direction;
        if (OnChangeDirection != null)
            OnChangeDirection(_direction);
    }
}
