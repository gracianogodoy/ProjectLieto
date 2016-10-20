using UnityEngine;

public class SquireAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Move _move;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _move = GetComponentInParent<Move>();
    }

    void Update()
    {
        _animator.SetBool("IsWalking", _move.enabled);
    }
}
