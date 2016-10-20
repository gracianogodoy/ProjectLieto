using UnityEngine;
using UnityEngine.Assertions;

public class Pushback : MonoBehaviour
{
    [SerializeField]
    private Vector2 _force;

    private CharacterMotor _motor;
    private Life _life;

    void Start()
    {
        _motor = GetComponent<CharacterMotor>();
        _life = GetComponent<Life>();

        Assert.IsNotNull(_motor);
        Assert.IsNotNull(_life);

        _life.OnTakeDamageFromObject += onTakeDamageFromObject;
    }

    private void onTakeDamageFromObject(int amount, GameObject sourceDamageObject)
    {
        var pushbackDirection = (Vector2)(transform.position - sourceDamageObject.transform.position).normalized;

        pushbackDirection.x *= _force.x;
        pushbackDirection.y = _force.y;

        _motor.Velocity = pushbackDirection;
    }
}
