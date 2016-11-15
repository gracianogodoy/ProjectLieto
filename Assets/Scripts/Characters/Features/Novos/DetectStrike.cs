using System;
using UnityEngine;

namespace GG
{
    public class DetectStrike : MonoBehaviour, IStrikeable
    {
        public Action<int, Vector2> OnStrike;

        public void Striked(int damage, Vector2 direction)
        {
            if (OnStrike != null)
                OnStrike(damage, direction);
        }
    }

}