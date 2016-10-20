using UnityEngine;

public class WorldSwitch : MonoBehaviour
{
    public delegate void CheckColliderEvent(Collider2D collider);

    [SerializeField]
    private GameObject _world1;
    [SerializeField]
    private GameObject _world2;

    public CheckColliderEvent OnCheckCollider { get; set; }

    public void Switch()
    {
        var worldToEnable = _world1.activeInHierarchy ? _world2 : _world1;
        var worldToDisable = _world1.activeInHierarchy ? _world1 : _world2;

        toggleWorldElement(worldToDisable.transform, false);
        worldToDisable.SetActive(false);
        toggleWorldElement(worldToEnable.transform, true);
        worldToEnable.SetActive(true);
    }

    private void toggleWorldElement(Transform worldElement, bool enable)
    {
        foreach (Transform child in worldElement)
        {
            var renderer = child.GetComponent<Renderer>();
            if (renderer != null)
                renderer.enabled = enable;

            var collider = child.GetComponent<Collider2D>();
            if (collider != null)
            {
                checkCollider(collider);
                collider.enabled = enable;
            }

            toggleWorldElement(child, enable);
        }
    }

    private void checkCollider(Collider2D collider)
    {
        if (OnCheckCollider != null)
            OnCheckCollider(collider);
    }
}
