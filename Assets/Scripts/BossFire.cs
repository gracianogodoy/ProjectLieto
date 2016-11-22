using UnityEngine;

public class BossFire : MonoBehaviour
{
    private Animator _animator;
    private Collider2D _collider;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();

        //LightOff();
    }

    public void LightOn()
    {
        _collider.enabled = true;
        _animator.SetTrigger("LightUp");
    }

    public void LightOff()
    {
        _collider.enabled = false;
        _animator.SetTrigger("TurnOff");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var strikeable = other.GetComponent<IStrikeable>();

            strikeable.Striked(1, Vector2.left);
        }
    }
}
