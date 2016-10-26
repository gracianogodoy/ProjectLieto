using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField]
    private Color _color;

    private WorldSwitch _worldSwitch;
    private SpriteRenderer[] _renderers;
    private bool _switch;

    void Start()
    {
        _worldSwitch = GameObject.FindObjectOfType<WorldSwitch>();
        _worldSwitch.OnChangeWorld += onChangeWorld;

        _renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void onChangeWorld()
    {
        var color = _switch ? Color.white : _color;
        changeColor(color);
        _switch = !_switch;
    }

    private void changeColor(Color color)
    {
        foreach (var renderer in _renderers)
        {
            renderer.color = color;
        }
    }
}
