using UnityEngine;
using UnityEngine.Assertions;

public class GolemAnimationController : MonoBehaviour
{
    private ProximitySensor _sensor;
    private Animator _animator;
    private Attack _attack;

    void Start()
    {
        _sensor = GetComponentInParent<ProximitySensor>();
        Assert.IsNotNull(_sensor);

        _animator = GetComponent<Animator>();
        Assert.IsNotNull(_animator);

        _attack = GetComponentInParent<Attack>();
        Assert.IsNotNull(_attack);

        _sensor.ReadySensor.OnEnterSensor += onEnterReadySensor;
        _sensor.ReadySensor.OnLeaveSensor += onLeaveReadySensor;
    }

    private void onEnterReadySensor(GameObject target)
    {
        _animator.SetBool("IsReady", true);
    }

    private void onLeaveReadySensor(GameObject target)
    {
        _animator.SetBool("IsReady", false);
    }

    void Update()
    {
        if (_attack.enabled)
            _animator.SetTrigger("Attack");
    }
}
