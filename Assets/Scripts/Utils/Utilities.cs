using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public static class Utilities
    {
        // public static Vector2 GetRandomPositionToSpawn(RectangleToSpawn positionsToSpawn)
        // {
        //     float randomPositionX = Random.Range(positionsToSpawn.MinPosition.x, positionsToSpawn.MaxPosition.x);
        //     float randomPositionY = Random.Range(positionsToSpawn.MinPosition.y, positionsToSpawn.MaxPosition.y);
        //
        //     return new Vector2(randomPositionX, randomPositionY);
        // }

        public static Vector2 ClampPosition(Vector2 screen, Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, -screen.x,
                screen.x);
            position.y = Mathf.Clamp(position.y, -screen.y+1,
                -1);
            return position;
        }
        
     public static int GetRandom(int min, int max)
    {
        return Random.Range(min, max);
    }

        public static float Truncate(float value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        double result = Math.Truncate(mult * value) / mult;
        return (float) result;
    }
        
        public static bool IsOnScreen(Camera camera, Transform transform)
        {
            Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 &&
                            screenPoint.y < 1;
            return onScreen;
        }

        // public static Vector2 GetRandomPositionToSpawn(RectangleToSpawn positionsToSpawn)
        // {
        //     float randomPositionX = Random.Range(positionsToSpawn.MinPosition.x, positionsToSpawn.MaxPosition.x);
        //     float randomPositionY = Random.Range(positionsToSpawn.MinPosition.y, positionsToSpawn.MaxPosition.y);
        //
        //     return new Vector2(randomPositionX, randomPositionY);
        // }

        public static float RoundValue(float value, int decimals)
        {
            return (float) Decimal.Round((decimal) value, decimals);
        }
        
        public static int GetRandomValue(int min, int max)
        {
            var randomValue = Random.Range(min, max);

            return randomValue;
        }

        public static Transform GetRandomPosition(List<Transform> positions)
        {
            if (positions.Count <= 0) return null;
            var randomPosition = GetRandomValue(0, positions.Count - 1);
            return positions[randomPosition];
        }

        public static bool IsLayerInLayerMask(LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        public static int GetMaximumValueInList(List<int> valuesToCheck)
        {
            return valuesToCheck.Prepend(Int32.MinValue).Max();
        }
    
        public static int GetMinimumValueInList(List<int> valuesToCheck)
        {
            return valuesToCheck.Prepend(Int32.MaxValue).Min();
        }
        
        //TODO REFRACTOR
        public static bool HasPastTime(float currentTime,float attackSpeed)
        {
            currentTime += Time.deltaTime;
            return currentTime > attackSpeed;
        }
    }
}