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

    void Start()
    {
        Timing.RunCoroutine(loadAll(_startMenuScenes, null, false));
    }

    public void StartGame()
    {
        Timing.RunCoroutine(loadAll(_scenes, _startMenuScenes, true));
    }

    public void BackToMenu()
    {
        Timing.RunCoroutine(loadAll(_startMenuScenes, _scenes, true));
    }

    private IEnumerator<float> loadAll(string[] scenes, string[] scenesToUnload, bool unload)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            var scene = scenes[i];

            yield return Timing.WaitUntilDone(Timing.RunCoroutine(load(scene)));
        }

        if (unload)
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
