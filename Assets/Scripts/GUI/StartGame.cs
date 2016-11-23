using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private AudioClip _startBGM;
    private LoadScene _loadScene;

    void Start()
    {
        _loadScene = GameObject.FindObjectOfType<LoadScene>();
        var backgroundMusic = SoundKit.instance.backgroundSound;

        if (backgroundMusic == null)
        {
            SoundKit.instance.playBackgroundMusic(_startBGM, 1);
        }
        else if (backgroundMusic.audioSource.clip != _startBGM)
            SoundKit.instance.playBackgroundMusic(_startBGM, 1);
    }

    public void OnStartGame()
    {
        _loadScene.Intro();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        _loadScene.BackToMenu();
    }

    public void Credits()
    {
        _loadScene.Credits();
    }
}
