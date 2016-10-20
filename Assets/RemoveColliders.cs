using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RemoveColliders : MonoBehaviour
{

    public bool delete;

    void OnGUI()
    {
        foreach (Transform child in transform)
        {

            var collider = child.GetComponent<BoxCollider2D>();

            Debug.Log(child.name + collider);

            if (collider != null)
                DestroyImmediate(collider);
        }
    }
}
