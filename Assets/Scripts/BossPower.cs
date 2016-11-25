using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

public class BossPower : MonoBehaviour
{
    public float timeToExplode;
    public AudioClip explosionAudio;
    public float volume;
    private Animator _animator;
    private Collider2D _collider;

    private static bool _isPlaying;

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

        if (!_isPlaying)
        {
            var sound = SoundKit.instance.playSound(explosionAudio, volume);
            _isPlaying = true;
            Timing.RunCoroutine(wait());
        }

        yield return Timing.WaitForSeconds(0.667f);

        Destroy(gameObject);
    }

    private IEnumerator<float> wait()
    {
        yield return Timing.WaitForSeconds(0.1f);

        _isPlaying = false;
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
