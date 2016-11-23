using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField]
    private Color _color;

    [Zenject.Inject]
    private GG.SwitchWorld _world;

    private SpriteRenderer[] _renderers;
    private GameObject[] _props;
    private bool _switch;

    void Start()
    {
        _world.OnSwitch += onChangeWorld;

        _renderers = GetComponentsInChildren<SpriteRenderer>();
        _props = GameObject.FindGameObjectsWithTag("BGProps");

        enableProps(false);
    }

    private void onChangeWorld()
    {
        var color = _switch ? Color.white : _color;
        changeColor(color);
        enableProps(!_switch);
        _switch = !_switch;
    }

    private void changeColor(Color color)
    {
        foreach (var renderer in _renderers)
        {
            renderer.color = color;
        }
    }

    private void enableProps(bool value)
    {
        foreach (var p in _props)
        {
            p.SetActive(value);
        }
    }
}
