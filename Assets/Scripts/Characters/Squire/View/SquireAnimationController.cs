using UnityEngine;

public class SquireAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Move _move;
    private Life _life;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _move = GetComponentInParent<Move>();
        _life = GetComponentInParent<Life>();
        _life.OnDead += onDead;
    }

    void Update()
    {
        _animator.SetBool("IsWalking", _move.enabled);
    }

    private void onDead()
    {
        _animator.SetTrigger("Die");
    }
}
