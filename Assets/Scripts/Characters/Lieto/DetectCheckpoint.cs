using UnityEngine;
using System;
using System.Collections.Generic;

public class DetectCheckpoint : MonoBehaviour
{
    public Action<GameObject> OnDetect;

    private List<GameObject> _knownCheckpoints = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint")
        {
            if (_knownCheckpoints.Contains(other.gameObject))
                return;

            _knownCheckpoints.Add(other.gameObject);

            if (OnDetect != null)
                OnDetect(other.gameObject);
        }
    }
}
