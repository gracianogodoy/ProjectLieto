using UnityEngine;
using Zenject;

namespace GG
{
    public class Lieto : MonoBehaviour, IInitializable
    {
        private Vector2 _startPosition;

        public Vector2 StartPosition { get { return _startPosition; } set { _startPosition = value; } }

        public void Initialize()
        {
            _startPosition = transform.localPosition;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Checkpoint")
            {
                _startPosition = other.transform.localPosition;
            }
        }
    }
}