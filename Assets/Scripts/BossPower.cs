using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

public class BossPower : MonoBehaviour
{
    public float timeToExplode;
    public AudioClip explosionAudio;
    private Animator _animator;
    private Collider2D _collider;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        Timing.RunCoroutine(startPower());
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
    }

    private IEnumerator<float> startPower()
    {
        _animator.SetTrigger("Start");

        yield return Timing.WaitForSeconds(timeToExplode);

        _animator.SetTrigger("Explode");
        _collider.enabled = true;
        SoundKit.instance.playSound(explosionAudio);

        yield return Timing.WaitForSeconds(0.667f);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var strikeable = other.GetComponent<IStrikeable>();

            var force = new Vector2(0.3f, 0.3f);

            strikeable.Striked(1, force);
        }
    }
}
