using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class DamageBlink : MonoBehaviour
{
    [SerializeField]
    private Color _blinkColor;
    [SerializeField]
    private float _numberOfBlinks;
    [SerializeField]
    private float _intervalBetweenBlinks;

    private Life _life;
    private bool _isBlinking;
    private SpriteRenderer _renderer;

    void Start()
    {
        _life = GetComponentInParent<Life>();
        Assert.IsNotNull(_life);

        _life.OnTakeDamage += onTakeDamage;

        _renderer = GetComponent<SpriteRenderer>();
    }

    private void onTakeDamage(int amount)
    {
        if (!_isBlinking)
        {
            _isBlinking = true;
            StartCoroutine(blink());
        }
    }

    private IEnumerator blink()
    {
        var currentBlink = 0;
        var changeColor = false;

        while (currentBlink < _numberOfBlinks)
        {
            _renderer.color = changeColor ? Color.white : _blinkColor;

            yield return new WaitForSeconds(_intervalBetweenBlinks);

            if (!changeColor)
                currentBlink++;

            changeColor = !changeColor;
        }

        _renderer.color = Color.white;
        _isBlinking = false;
    }
}

