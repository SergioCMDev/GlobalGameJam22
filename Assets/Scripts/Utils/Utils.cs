using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UtilUnity
{
    public static class Utils
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
    }
}