using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScenes : MonoBehaviour
{
    void Start()

    {
        SceneManager.LoadScene("core");

        SceneManager.LoadScene("bootcamp2", LoadSceneMode.Additive);
        SceneManager.LoadScene("bg", LoadSceneMode.Additive);

        //StartCoroutine(AsynchronousLoad("core"));
    }

    private IEnumerator LoadALevel()
    {
        AsyncOperation _async = SceneManager.LoadSceneAsync("core");
        Debug.Log(_async.progress);
        yield return _async;
    }

    IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Loading completed
            if (ao.progress == 0.9f)
            {
                Debug.Log("Press a key to start");
                if (Input.GetKeyDown(KeyCode.Space))
                    ao.allowSceneActivation = true;
            }

            yield return null;
        }
        Debug.Log("ok");
        SceneManager.LoadScene("scene1", LoadSceneMode.Additive);
    }


}
