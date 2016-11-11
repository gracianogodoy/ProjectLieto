using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMapClone))]
public class TileMapCloneEditor : Editor
{
    private TileMapClone _tileMapClone;

    void OnEnable()
    {
        _tileMapClone = target as TileMapClone;

        _tileMapClone.Setup();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Clone Safe"))
        {
            _tileMapClone.Clone(TileMapClone.Mode.Safe);
        }
        GUILayout.Space(5);

        if (GUILayout.Button("Clone Safe Override"))
        {
            _tileMapClone.Clone(TileMapClone.Mode.SafeOverride);
        }

        GUILayout.Space(45);
        if (GUILayout.Button("Clone Override"))
        {
            _tileMapClone.Clone(TileMapClone.Mode.Override);
        }
    }
}
