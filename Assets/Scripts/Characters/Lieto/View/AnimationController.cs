using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    [Inject]
    private GG.CharacterMotor _motor;
    [Inject]
    private GG.Move _move;
    [Inject]
    private GG.Jump _jump;
    [Inject]
    private GG.Attack _attack;
    [Inject]
    private GG.Life _life;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();

        _life.OnDead += () =>
          {
              _animator.SetTrigger("Dead");
          };

        _life.OnRessurect += () =>
        {
            _animator.SetTrigger("Alive");
        };

        _jump.OnJump += () => _animator.SetBool("IsJumping", true);
        _jump.OnStopJump += () => _animator.SetBool("IsJumping", false);
    }

    void Update()
    {
        _animator.SetBool("IsMoving", Mathf.Abs(_motor.Velocity.x) > 0.01f);
        _animator.SetBool("IsFalling", !_motor.IsGrounded && _motor.Velocity.y < 0);
        _animator.SetBool("IsAttacking", _attack.IsAttacking);
    }
}
