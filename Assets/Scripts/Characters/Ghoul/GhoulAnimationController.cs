using UnityEngine;
using UnityEngine.Assertions;

public class GhoulAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Attack _attack;
    private Life _life;

    private bool _doAttack;

    void Start()
    {
        _animator = GetComponent<Animator>();
        Assert.IsNotNull(_animator);

        _attack = GetComponentInParent<Attack>();
        Assert.IsNotNull(_attack);

        _life = GetComponentInParent<Life>();
        _life.OnDead += onDead;

    }

    void Update()
    {
        if (_attack.enabled && !_doAttack)
        {
            _animator.SetTrigger("Attack");
            _doAttack = true;
        }

        if (!_attack.enabled && _doAttack)
            _doAttack = false;
    }

    private void onDead()
    {
        _animator.SetTrigger("Die");
    }
}
