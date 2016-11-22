using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private AudioClip _startBGM;
    private LoadScene _loadScene;

    void Start()
    {
        _loadScene = GameObject.FindObjectOfType<LoadScene>();
        SoundKit.instance.playBackgroundMusic(_startBGM, 1);
    }

    public void OnStartGame()
    {
        _loadScene.StartGame();
    }
}
