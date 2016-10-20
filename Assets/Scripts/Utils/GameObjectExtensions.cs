using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public static class GameObjectExtensions
{
    public static GameObject[] RaycastMany(this GameObject go, Vector2 origin, Vector2 direction, float distance,
        LayerMask hitLayers, float baseSize, int numberOfRays)
    {
        var faceDirectionInverted = new Vector2(direction.y, direction.x);
        var firstRayOrigin = origin + faceDirectionInverted * baseSize / 2;
        float distanceBetweenRays = baseSize / (numberOfRays - 1);

        var gameObjects = new List<GameObject>();
        var tempHits = new List<RaycastHit2D>();

        for (int i = 0; i < numberOfRays; i++)
        {
            var hit = Physics2D.RaycastAll(firstRayOrigin, direction, distance, hitLayers);
            Debug.DrawRay(firstRayOrigin, direction * distance, Color.red);

            tempHits.AddRange(hit);

            firstRayOrigin -= faceDirectionInverted * distanceBetweenRays;
        }

        for (int i = 0; i < tempHits.Count; i++)
        {
            var gameObject = tempHits[i].collider.gameObject;
            if (!gameObjects.Contains(gameObject))
            {
                if (gameObject != go)
                    gameObjects.Add(gameObject);
            }
        }

        return gameObjects.ToArray();
    }
}
