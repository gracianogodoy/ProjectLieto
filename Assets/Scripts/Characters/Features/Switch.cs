using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{
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
        _worldSwitch.OnCheckCollider = onCheckCollider;

        _collider = GetComponent<BoxCollider2D>();
        _motor = GetComponent<CharacterMotor>();
        _life = GetComponent<Life>();
    }

    public void DoSwitch()
    {
        _worldSwitch.Switch();
    }

    private void onCheckCollider(Collider2D collider)
    {
        var offset = 0.01f;
        var myBounds = _collider.bounds;
        myBounds.size = new Vector3(myBounds.size.x - offset, myBounds.size.y - offset);

        if (collider.bounds.Intersects(myBounds))
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
