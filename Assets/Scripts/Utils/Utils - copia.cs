using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Util
{
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
}