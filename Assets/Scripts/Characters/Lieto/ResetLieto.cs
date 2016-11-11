using UnityEngine;
using System.Collections;

public class ResetLieto : MonoBehaviour
{
    [SerializeField]
    private float _timeToReset;

    private Life _life;
    private CheckpointController _checkpoint;
    private Animator _animator;
    private Switch _switch;

    void Start()
    {
        _life = GetComponent<Life>();
        _life.OnDead += onDead;
        _animator = GetComponentInChildren<Animator>();
        _switch = GetComponent<Switch>();

        _checkpoint = GetComponent<CheckpointController>();
    }

    private void onDead()
    {
        StartCoroutine(reset());
        if (_switch.IsOnBadWorld)
        {
            _switch.DoSwitch();
        }
    }

    private IEnumerator reset()
    {
        yield return new WaitForSeconds(_timeToReset);
        _life.Ressurect();
        _animator.SetTrigger("Alive");
        moveToPlace();
    }

    private void moveToPlace()
    {
        var lastCheckpoint = _checkpoint.LastCheckpoint;

        if (lastCheckpoint != null)
        {
            transform.position = new Vector3(lastCheckpoint.x, lastCheckpoint.y + 0.5f, transform.position.z);
        }
    }
}
