using UnityEngine;
using System.Linq;

public class ScrollBackground : MonoBehaviour
{
    public float Speed;

    private Renderer[] _renderers;

    void Start()
    {
        _renderers = (from r in GetComponentsInChildren<Renderer>()
                      select r).ToArray();
    }

    void Update()
    {
        float scrollSpeed = Time.time * Speed;

        foreach (var renderer in _renderers)
        {
            renderer.material.mainTextureOffset = new Vector2(scrollSpeed, 0);
        }
    }
}
