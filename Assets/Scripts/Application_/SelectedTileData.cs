using System;
using UnityEngine;

namespace Application_
{
    [Serializable]
    public class SelectedTileData
    {
        public Vector3Int GridPosition;
        public TileInnerData TileInnerData;
    }
}