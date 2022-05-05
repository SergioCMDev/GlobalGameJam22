using System;
using UnityEngine;

namespace Presentation
{
    [Serializable]
    public class TilePosition
    {
        public Vector3Int GridPosition;
        [HideInInspector] public Vector3 WorldPosition;
    }
}