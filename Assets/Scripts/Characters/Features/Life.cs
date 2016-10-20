using UnityEngine;

public class Life : MonoBehaviour
{
    public delegate void DeadEvent();
    public delegate void TakeDamageEvent(int damage);
    public delegate void TakeDamageFromObjectEvent(int damage, GameObject damageSourceObject);

    [SerializeField]
    private int _totalLife;
    [SerializeField]
    private MonoBehaviour[] _scriptsToDisable;

    private int _currentLife;

    public DeadEvent OnDead { get; set; }
    public TakeDamageEvent OnTakeDamage { get; set; }
    public TakeDamageFromObjectEvent OnTakeDamageFromObject { get; set; }

    void Start()
    {
        _currentLife = _totalLife;
    }

    public void TakeDamage(int amount)
    {
        damage(amount, null);
    }

    public void TakeDamage(int amount, GameObject damageSourceObject)
    {
        damage(amount, damageSourceObject);
    }

    private void damage(int amount, GameObject damageSourceObject)
    {
        if (_currentLife <= 0)
            return;

        changeLife(-amount);

        if (damageSourceObject)
            if (OnTakeDamageFromObject != null)
                OnTakeDamageFromObject(amount, damageSourceObject);

        if (OnTakeDamage != null)
            OnTakeDamage(amount);

        if (_currentLife <= 0)
            dead();
    }

    public void Heal(int amount)
    {
        changeLife(amount);
    }

    private void changeLife(int amount)
    {
        _currentLife += amount;

        _currentLife = Mathf.Clamp(_currentLife, 0, _totalLife);
    }

    private void dead()
    {
        if (OnDead != null)
            OnDead();

        foreach (var script in _scriptsToDisable)
        {
            script.enabled = false;
        }
    }
}
