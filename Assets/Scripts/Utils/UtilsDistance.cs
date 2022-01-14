using UnityEngine;

public static class UtilsDistance
{
    public static float GetDistanceBetweenTwoPoints(Vector3 positionA, Vector3 positionB)
    {
        Vector3 offset = positionB - positionA;
        float distance = offset.sqrMagnitude;
        return distance;
    }
}