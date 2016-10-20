using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "SceneSetupInfo")]
public class SceneSetupInfo : ScriptableObject
{
    [SerializeField]
    private SceneAsset[] _scenes;

    public SceneSetup[] ScenesToLoad
    {
        get
        {
            var scenesToLoad = new SceneSetup[_scenes.Length];

            for (int i = 0; i < _scenes.Length; i++)
            {
                string path = AssetDatabase.GetAssetPath(_scenes[i]);

                var sceneToLoad = new SceneSetup();
                sceneToLoad.path = path;
                sceneToLoad.isLoaded = true;

                if (i == 0)
                    sceneToLoad.isActive = true;

                scenesToLoad[i] = sceneToLoad;
            }

            return scenesToLoad;
        }
    }
}
