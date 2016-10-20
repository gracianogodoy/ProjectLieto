using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Assertions;
using System.Linq;

public class MultipleSceneSetup : EditorWindow
{
    private SceneSetupInfo _sceneSetupInfo;

    [MenuItem("Window/Scene Setup")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MultipleSceneSetup));
    }

    void OnGUI()
    {
        _sceneSetupInfo = (SceneSetupInfo)EditorGUILayout.ObjectField("SceneSetupInfo", _sceneSetupInfo, typeof(SceneSetupInfo), false);

        //if (GUILayout.Button("Save From Current"))
        //{
        //    saveFromCurrentButton();
        //}

        if (GUILayout.Button("LoadAll"))
        {
            loadAll();
        }
    }

    private void saveFromCurrentButton()
    {
        var sceneSetups = EditorSceneManager.GetSceneManagerSetup();
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Debug.Log(path);
        var activeScene = (from s in sceneSetups
                           where s.isActive
                           select s).FirstOrDefault();
        var sceneSetupInfo = ScriptableObject.CreateInstance<SceneSetupInfo>();
        //AssetDatabase.CreateAsset(sceneSetupInfo, path + "/core.asset");
    }

    private void loadAll()
    {
        Assert.IsNotNull(_sceneSetupInfo);
        EditorSceneManager.RestoreSceneManagerSetup(_sceneSetupInfo.ScenesToLoad);
    }
}
