using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using MovementEffects;
using UnityEngine.Events;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string[] _scenes;
    [SerializeField]
    private string[] _startMenuScenes;

    public UnityEvent OnLoad;

    public void StartGame()
    {
        Timing.RunCoroutine(loadAll(_scenes, _startMenuScenes));
    }

    public void BackToMenu()
    {
        Timing.RunCoroutine(loadAll(_startMenuScenes, _scenes));
    }

    private IEnumerator<float> loadAll(string[] scenes, string[] scenesToUnload)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            var scene = scenes[i];

            yield return Timing.WaitUntilDone(Timing.RunCoroutine(load(scene)));
        }

        for (int i = 0; i < scenesToUnload.Length; i++)
        {
            var scene = scenesToUnload[i];
            SceneManager.UnloadScene(scene);
        }

        if (OnLoad != null)
            OnLoad.Invoke();
    }

    private IEnumerator<float> load(string scene)
    {
        var async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!async.isDone)
        {
            yield return 0;
        }
    }
}
