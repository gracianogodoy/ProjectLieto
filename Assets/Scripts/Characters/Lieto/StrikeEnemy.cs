using UnityEngine;
using UnityEngine.Assertions;

public class StrikeEnemy : MonoBehaviour
{
    [SerializeField]
    private int _damage;

    private Attack _attack;

    void Start()
    {
        _attack = GetComponent<Attack>();
        Assert.IsNotNull(_attack);

        _attack.OnAttackHit += onAttackHit;
    }

    private void onAttackHit(GameObject[] hits)
    {
        foreach (var hit in hits)
        {
            var strikeable = hit.GetComponent<IStrikeable>();

            //if (strikeable != null)
            //    strikeable.Striked(_damage, gameObject);
        }
    }
}
