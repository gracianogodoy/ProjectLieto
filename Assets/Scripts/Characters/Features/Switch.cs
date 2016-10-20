using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{
    [SerializeField]
    private LayerMask _obstacleMask;
    [SerializeField]
    private int _damageOnOverlapping;
    [SerializeField]
    private float _timeToSwitchBackAfterDamage;

    private WorldSwitch _worldSwitch;
    private BoxCollider2D _collider;
    private bool _isTouching;
    private CharacterMotor _motor;
    private Life _life;

    void Start()
    {
        _worldSwitch = GameObject.FindObjectOfType<WorldSwitch>();

        _collider = GetComponent<BoxCollider2D>();
        _motor = GetComponent<CharacterMotor>();
        _life = GetComponent<Life>();
    }

    public void DoSwitch()
    {
        _worldSwitch.Switch();
        checkForCollider();
    }

    private void checkForCollider()
    {
        var offset = 0.1f;
        var myBounds = _collider.bounds;
        myBounds.size = new Vector3(myBounds.size.x - offset, myBounds.size.y - offset);

        var hit = Physics2D.BoxCast(myBounds.center, myBounds.size, 0, Vector2.zero, 0, _obstacleMask);

        if (hit)
        {
            _motor.enabled = false;
            _motor.Velocity = Vector2.zero;
            _isTouching = true;

            StartCoroutine(waitToSwitchBack(_timeToSwitchBackAfterDamage));
        }
    }

    private IEnumerator waitToSwitchBack(float time)
    {
        yield return new WaitForSeconds(time);

        if (_isTouching)
        {
            _worldSwitch.Switch();
            _life.TakeDamage(_damageOnOverlapping);
            _isTouching = false;
            _motor.enabled = true;
        }
    }
}
