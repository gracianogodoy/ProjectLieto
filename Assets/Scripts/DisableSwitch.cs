using UnityEngine;
using System.Collections;
using Zenject;
namespace GG
{
    public class DisableSwitch : MonoBehaviour
    {
        [Inject]
        private DisableSwitchSignal _disable;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                _disable.Fire();
            }
        }
    }
}
