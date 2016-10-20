using UnityEngine;
using System.Linq;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private string _horizontalAxis;
    [SerializeField]
    private string _jumpButton;
    [SerializeField]
    private string _attackButton;
    [SerializeField]
    private string _worldSwitchButton;

    void Update()
    {
        moveCommand();
        jumpCommand();
        attackCommand();
        switchWorldCommand();
    }

    private void moveCommand()
    {
        var axis = Input.GetAxisRaw(_horizontalAxis);

        var move = GetComponent<Move>();
        var faceDirection = GetComponent<FaceDirection>();
        var attack = GetComponent<Attack>();

        if (axis != 0)
        {
            move.DoMove((int)axis);

            faceDirection.SetDirection((int)axis);
        }
        else
            move.StopMove();

        if (attack.enabled)
            move.StopMove();
    }

    private void jumpCommand()
    {
        var jump = GetComponent<Jump>();

        if (Input.GetButtonDown(_jumpButton))
            jump.DoJump();

        if (Input.GetButtonUp(_jumpButton))
            jump.StopJump();
    }

    private void attackCommand()
    {
        var attack = GetComponent<Attack>();
        var jump = GetComponent<Jump>();

        var faceDirection = GetComponent<FaceDirection>();
        if (Input.GetButtonDown(_attackButton) && !jump.enabled)
            attack.DoAttack(faceDirection.Direction);
    }

    private void switchWorldCommand()
    {
        var switchWorld = GetComponent<Switch>();

        if (Input.GetButtonDown(_worldSwitchButton))
        {
            switchWorld.DoSwitch();
        }
    }
}
