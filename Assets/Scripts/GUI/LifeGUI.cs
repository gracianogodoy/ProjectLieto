using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GG
{
    public class LifeGUI : MonoBehaviour
    {
        private Life _life;
        private Image[] _fullHearts;

        [Inject]
        public void Construct(Life life)
        {
            _life = life;
        }

        void Start()
        {
            var fullHeartPanel = transform.Find("full_hearts");

            _fullHearts = fullHeartPanel.GetComponentsInChildren<Image>();

            _life.OnTakeDamage += onTakeDamage;
            _life.OnHeal += onHeal;
            _life.OnResurrect += setHearts;
        }

        private void onHeal(int amount)
        {
            setHearts();
        }

        private void onTakeDamage(int amount)
        {
            setHearts();
        }

        private void setHearts()
        {
            foreach (var heart in _fullHearts)
            {
                heart.enabled = false;
            }

            for (int i = 0; i < _life.CurrentLife; i++)
            {
                _fullHearts[i].enabled = true;
            }
        }
    }
}