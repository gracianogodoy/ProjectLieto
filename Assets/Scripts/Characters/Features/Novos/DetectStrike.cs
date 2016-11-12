using System;
using UnityEngine;

namespace GG
{
    public class DetectStrike : MonoBehaviour, IStrikeable
    {
        public Action<int, GameObject> OnStrike;

        public void Striked(int damage, GameObject other)
        {
            if (OnStrike != null)
                OnStrike(damage, other);
        }
    }

}