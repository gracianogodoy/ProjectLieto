using System.Collections;
using UnityEngine;

public class WorldSwitch : MonoBehaviour
{
    [SerializeField]
    private int _numberOfGlitches;
    [SerializeField]
    private float _intervalBetweenGlitches;
    [SerializeField]
    private GameObject _world1;
    [SerializeField]
    private GameObject _world2;

    private bool _isOnFirstWorld = true;

    public void Switch()
    {
        StartCoroutine(glitch());
    }

    private bool startSwitch()
    {
        var worldToEnable = _world1.activeInHierarchy ? _world2 : _world1;
        var worldToDisable = _world1.activeInHierarchy ? _world1 : _world2;

        toggleWorldElement(worldToDisable.transform, false);
        worldToDisable.SetActive(false);
        toggleWorldElement(worldToEnable.transform, true);
        worldToEnable.SetActive(true);

        return _world1.activeInHierarchy;
    }

    private void toggleWorldElement(Transform worldElement, bool enable)
    {
        foreach (Transform child in worldElement)
        {
            var renderer = child.GetComponent<Renderer>();
            if (renderer != null)
                renderer.enabled = enable;

            toggleWorldElement(child, enable);
        }
    }

    private IEnumerator glitch()
    {
        var currentGlitch = 0;
        var changeColor = false;

        while (currentGlitch < _numberOfGlitches)
        {
            changeColor = startSwitch();

            yield return new WaitForSeconds(_intervalBetweenGlitches);

            if (!changeColor)
                currentGlitch++;

            changeColor = !changeColor;
        }

        if (!_isOnFirstWorld)
            startSwitch();

        _isOnFirstWorld = !_isOnFirstWorld;
    }
}
