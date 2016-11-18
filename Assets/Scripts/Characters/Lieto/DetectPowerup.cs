using UnityEngine;
using System;

public class DetectPowerup : MonoBehaviour
{
    public Action OnDetect;

    [Zenject.Inject]
    private GG.Life _life;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Powerup" && !_life.IsFull)
        {
            if (OnDetect != null)
                OnDetect();

            Destroy(other.gameObject);
        }
    }
}
