using UnityEngine;

namespace GG
{
    public class AdvanceStory : MonoBehaviour
    {
        [Zenject.Inject]
        private StoryFlow _storyFlow;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                _storyFlow.NextAct();
            }
        }
    }
}