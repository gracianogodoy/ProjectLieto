using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private CharacterMotor _motor;
    private Move _move;
    private Jump _jump;
    private Attack _attack;
    private Life _life;
    private Animator _animator;

    void Start()
    {
        _motor = GetComponentInParent<CharacterMotor>();
        _move = GetComponentInParent<Move>();
        _jump = GetComponentInParent<Jump>();
        _attack = GetComponentInParent<Attack>();
        _life = GetComponentInParent<Life>();
        _animator = GetComponent<Animator>();

        _life.OnDead += ()=> _animator.SetTrigger("Dead");
    }

    void Update()
    {
        _animator.SetBool("IsMoving", Mathf.Abs(_motor.Velocity.x) > 0.01f);
        _animator.SetBool("IsJumping", _jump.enabled);
        _animator.SetBool("IsFalling", !_motor.IsGrounded && _motor.Velocity.y < 0);
        _animator.SetBool("IsAttacking", _attack.enabled);
    }
}
