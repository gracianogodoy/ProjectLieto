using UnityEngine;
using System.Collections.Generic;
using MovementEffects;

public class IntroScene : MonoBehaviour
{
    public AudioClip bgm;

    void Start()
    {
        SoundKit.instance.playBackgroundMusic(bgm, 1);
        Timing.RunCoroutine(wait());
    }

    private IEnumerator<float> wait()
    {
        yield return Timing.WaitForSeconds(3);

        var loadScene = GameObject.FindObjectOfType<LoadScene>();

        loadScene.StartGame();
    }
}
