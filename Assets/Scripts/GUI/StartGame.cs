using UnityEngine;

public class StartGame : MonoBehaviour
{
    private LoadScene _loadScene;

    void Start()
    {
        _loadScene = GameObject.FindObjectOfType<LoadScene>();
    }

    public void OnStartGame()
    {
        _loadScene.StartGame();
    }
}
