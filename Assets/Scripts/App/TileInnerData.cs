using System;
using UnityEngine;

namespace App
{
    [Serializable]
    public class TileInnerData
    {
        public bool Occupied;

        public TileInnerData()
        {
            Occupied = false;
            CanBeUsed = true;
        }

        public bool CanBeUsed;
        public GameObject OccupiedBy;
    }
}