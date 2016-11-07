using UnityEngine;

public class BossViewController : MonoBehaviour
{
    private Life _life;
    private Animator _animator;

    void Start()
    {
        _life = GetComponentInParent<Life>();
        _animator = GetComponent<Animator>();

        _life.OnDead += onDead;
        _life.OnTakeDamage += onTakeDamage;
    }

    private void onTakeDamage(int damage)
    {
        if (_life.CurrentLife > 0)
            _animator.SetTrigger("Hit");
    }

    private void onDead()
    {
        _animator.SetTrigger("Dead");
    }
}
