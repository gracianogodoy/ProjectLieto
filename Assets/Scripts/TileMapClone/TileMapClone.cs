using System.Collections.Generic;
using UnityEngine;

public class TileMapClone : MonoBehaviour
{
    public enum Mode
    {
        Safe,
        Override,
        SafeOverride,
    }

    [SerializeField]
    private tk2dTileMap _tileMapTarget;
    [SerializeField]
    private int _offset;

    private tk2dTileMap _tileMap;

    private Dictionary<int, int> _targetNameIndexes;
    private Dictionary<int, int> _myNameIndexes;

    public void Setup()
    {
        _tileMap = GetComponent<tk2dTileMap>();
        _targetNameIndexes = new Dictionary<int, int>();
        _myNameIndexes = new Dictionary<int, int>();
        createNames();
    }

    private void createNames()
    {
        for (int i = 0; i < 104; i++)
        {
            var name = "tileset/" + i;
            var id = _tileMapTarget.Editor__SpriteCollection.GetSpriteIdByName(name);
            _targetNameIndexes[id] = i;

            id = _tileMap.Editor__SpriteCollection.GetSpriteIdByName(name);
            _myNameIndexes[id] = i;
        }
    }

    public void Clone(Mode mode)
    {
        var layers = _tileMapTarget.Layers;

        for (int l = 0; l < layers.Length; l++)
        {
            var layer = layers[l];

            for (int w = 0; w < _tileMapTarget.width; w++)
            {
                for (int h = 0; h < _tileMapTarget.height; h++)
                {
                    var tileId = _tileMapTarget.GetTile(w, h, l);

                    if (tileId != -1)
                    {
                        var tileInfo = _tileMapTarget.GetTileInfoForTileId(tileId);

                        if (tileInfo.stringVal == "n")
                            continue;

                        var nameIndex = _targetNameIndexes[tileId];

                        var tileToChange = 0;

                        if (tileInfo.stringVal == "c")
                        {
                            tileToChange = getSpriteId("tileset/" + tileInfo.intVal);
                        }

                        if (tileInfo.stringVal == "")
                        {
                            tileToChange = getSpriteId("tileset/" + (nameIndex + _offset));
                        }


                        switch (mode)
                        {
                            case Mode.Safe:
                                cloneSafe(tileId, tileToChange, w, h, l);
                                break;
                            case Mode.Override:
                                cloneOverride(tileToChange, w, h, l);
                                break;
                            case Mode.SafeOverride:
                                cloneSafeAdd(tileId, tileToChange, w, h, l);
                                break;
                        }
                    }
                }
            }
        }

        _tileMap.Build();
    }

    private int getSpriteId(string name)
    {
        var collection = _tileMap.Editor__SpriteCollection;

        return collection.GetSpriteIdByName(name);
    }

    private void cloneOverride(int tileToChange, int x, int y, int layer)
    {
        _tileMap.SetTile(x, y, layer, tileToChange);
    }

    private void cloneSafe(int otherTileId, int tileToChange, int x, int y, int layer)
    {
        var targetTileId = _tileMapTarget.GetTile(x, y, layer);
        var myTileId = _tileMap.GetTile(x, y, layer);

        var targetNameIndex = _targetNameIndexes[targetTileId];
        var myNameIndex = _myNameIndexes[targetTileId];

        var targetTileName = "tileset/" + targetTileId;
        var myTileName = "tileset/" + myTileId;

        if (targetTileName == myTileName)
            _tileMap.SetTile(x, y, layer, tileToChange);
    }

    private void cloneSafeAdd(int otherTileId, int tileToChange, int x, int y, int layer)
    {
        var targetTileId = _tileMapTarget.GetTile(x, y, layer);
        var myTileId = _tileMap.GetTile(x, y, layer);

        var targetNameIndex = _targetNameIndexes[targetTileId];
        var myNameIndex = _myNameIndexes[targetTileId];

        var targetTileName = "tileset/" + targetTileId;
        var myTileName = "tileset/" + myTileId;

        if (targetTileName == myTileName || myTileId == -1)
            _tileMap.SetTile(x, y, layer, tileToChange);
    }
}
