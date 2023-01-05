using System;
using UnityEngine;

namespace App.Info.Tiles
{
    [Serializable]
    public class TilePosition
    {
        public Vector3Int GridPosition;
        [HideInInspector] public Vector3 WorldPosition;
    }
}