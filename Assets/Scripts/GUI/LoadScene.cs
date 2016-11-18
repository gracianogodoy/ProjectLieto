using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using MovementEffects;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string[] _scenes;

    public void StartGame()
    {
        Timing.RunCoroutine(loadAll());
    }

    private IEnumerator<float> loadAll()
    {
        for (int i = 0; i < _scenes.Length; i++)
        {
            var scene = _scenes[i];

            yield return Timing.WaitUntilDone(Timing.RunCoroutine(load(scene)));
        }

        SceneManager.UnloadScene("start");
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
