using UnityEngine;
using UnityEngine.Assertions;

public class GolemAnimationController : MonoBehaviour
{
    private ProximitySensor _sensor;
    private Animator _animator;
    private Attack _attack;
    private Life _life;

    private bool _doAttack;

    void Start()
    {
        _sensor = GetComponentInParent<ProximitySensor>();
        Assert.IsNotNull(_sensor);

        _animator = GetComponent<Animator>();
        Assert.IsNotNull(_animator);

        _attack = GetComponentInParent<Attack>();
        Assert.IsNotNull(_attack);

        _life = GetComponentInParent<Life>();
        _life.OnDead += onDead;

        _sensor.ReadySensor.OnEnterSensor += onEnterReadySensor;
        _sensor.ReadySensor.OnLeaveSensor += onLeaveReadySensor;
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

    private void onEnterReadySensor(GameObject target)
    {
        _animator.SetBool("IsReady", true);
    }

    private void onLeaveReadySensor(GameObject target)
    {
        _animator.SetBool("IsReady", false);
    }

    private void onDead()
    {
        _animator.SetTrigger("Die");
    }
}
