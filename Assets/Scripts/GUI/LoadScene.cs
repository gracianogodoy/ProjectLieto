using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using MovementEffects;
using UnityEngine.Events;
using System.Linq;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string[] _scenes;
    [SerializeField]
    private string[] _startMenuScenes;
    [SerializeField]
    private string[] _creditsScene;
    [SerializeField]
    private string[] _introScenes;

    public UnityEvent OnLoad;

    void Start()
    {
#if !UNITY_EDITOR
        BackToMenu();
#endif
    }

    public void StartGame()
    {
        Timing.RunCoroutine(loadAll(_scenes));
    }

    public void BackToMenu()
    {
        Timing.RunCoroutine(loadAll(_startMenuScenes));
    }

    public void Credits()
    {
        Timing.RunCoroutine(loadAll(_creditsScene));
    }

    public void Intro()
    {
        Timing.RunCoroutine(loadAll(_introScenes));
    }

    private IEnumerator<float> loadAll(string[] scenes)
    {
        var scenesToUnload = (from s in getLoadedScenes()
                              where s.name != SceneManager.GetActiveScene().name
                              select s).ToArray();

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

    private Scene[] getLoadedScenes()
    {
        List<Scene> scenes = new List<Scene>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            scenes.Add(scene);
        }

        return scenes.ToArray();
    }
}
