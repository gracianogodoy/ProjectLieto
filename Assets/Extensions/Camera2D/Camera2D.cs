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
        if (!enabled)
            return;

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

        letterBox();
    }

    private void letterBox()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }

    }

    [System.Serializable]
    public class Resolution
    {
        public float Width = 20;
        public float Height = 20;
    }
}
