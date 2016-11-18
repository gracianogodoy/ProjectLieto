using UnityEngine;

namespace GG
{
    public class PauseMenu : MonoBehaviour, Zenject.IInitializable, System.IDisposable
    {
        [Zenject.Inject]
        private PauseSignal _pauseSignal;
        [Zenject.Inject]
        private UnpauseSignal _unpauseSignal;

        public void Initialize()
        {
            _pauseSignal += onPause;
            _unpauseSignal += onUnpause;
        }

        public void Dispose()
        {
            _pauseSignal -= onPause;
            _unpauseSignal -= onUnpause;
        }

        public void BackToMenu()
        {
            var loadScene = GameObject.FindObjectOfType<LoadScene>();

            loadScene.BackToMenu();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void onPause()
        {
            gameObject.SetActive(true);

        }

        private void onUnpause()
        {
            gameObject.SetActive(false);
        }
    }
}