using System.Collections;
using UnityEngine;

public class WorldSwitch : MonoBehaviour
{
    public delegate void ChangeWorldEvent();

    [SerializeField]
    private float playingPositionY;
    [SerializeField]
    private float hidingPositionY;

    [SerializeField]
    private int _numberOfGlitches;
    [SerializeField]
    private float _intervalBetweenGlitches;
    [SerializeField]
    private GameObject _world1;
    [SerializeField]
    private GameObject _world2;

    private bool _isOnFirstWorld = true;
    private bool _isChanging;

    public ChangeWorldEvent OnChangeWorld { get; set; }

    public void Switch()
    {
        if (!_isChanging)
            StartCoroutine(glitch());
    }

    private bool startSwitch()
    {
        var world1playing = _world1.transform.position.y == playingPositionY;
        var worldToEnable = world1playing ? _world2 : _world1;
        var worldToDisable = world1playing ? _world1 : _world2;

        //toggleWorldElement(worldToDisable.transform, false);
        //worldToDisable.SetActive(false);
        //toggleWorldElement(worldToEnable.transform, true);
        //worldToEnable.SetActive(true);

        changePositionY(worldToDisable.transform, hidingPositionY);
        changePositionY(worldToEnable.transform, playingPositionY);

        if (OnChangeWorld != null)
            OnChangeWorld();

        world1playing = _world1.transform.position.y == playingPositionY;
        return world1playing;
    }

    private void changePositionY(Transform world, float positionY)
    {
        world.position = new Vector3(world.position.x, positionY, world.position.z);
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
        _isChanging = true;
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

        _isChanging = false;
    }
}
