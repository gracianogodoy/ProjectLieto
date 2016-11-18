using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class Camera2D : MonoBehaviour
{
    public Resolution NativeResolution;
    public int PixelPerUnit = 100;
    public bool FixedWidth;

    private Camera _camera;

    public Camera Camera
    {
        get
        {
            return _camera;
        }
    }

    void OnValidate()
    {
        _camera = GetComponent<Camera>();
        UnityEngine.Assertions.Assert.IsNotNull(_camera);

        calculateOrtographicSize();
    }

    void Start()
    {
        _camera = GetComponent<Camera>();
        UnityEngine.Assertions.Assert.IsNotNull(_camera);

        calculateOrtographicSize();
    }

    void OnGUI()
    {
        calculateOrtographicSize();
    }

    private void calculateOrtographicSize()
    {
        _camera.orthographic = true;

        if (FixedWidth)
        {
            float orthographicWidthSize = (NativeResolution.Width / (2 * _camera.aspect)) / PixelPerUnit;
            _camera.orthographicSize = orthographicWidthSize;
        }
        else
        {
            float orthographicHeightSize = (NativeResolution.Height / 2) / PixelPerUnit;
            _camera.orthographicSize = orthographicHeightSize;
        }
    }

    [System.Serializable]
    public class Resolution
    {
        public float Width = 20;
        public float Height = 20;
    }
}
