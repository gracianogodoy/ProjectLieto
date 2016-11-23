using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GG
{
    public class BossLifeView : MonoBehaviour
    {
        public Image lifeBar;
        [Inject]
        private Life _life;

        void Start()
        {
            _life.OnTakeDamage += onTakeDamage;
            _life.OnDead += onDead;
            _life.OnReset += onReset;
        }

        private void onReset()
        {
            lifeBar.transform.localScale = Vector3.one;
        }

        private void onDead()
        {
            lifeBar.transform.parent.gameObject.SetActive(false);
        }

        private void onTakeDamage(int obj)
        {
            var newScale = (float)_life.CurrentLife / (float)_life.TotalLife;
            lifeBar.transform.localScale = new Vector3(newScale, lifeBar.transform.localScale.y, lifeBar.transform.localScale.z);
        }
    }
}