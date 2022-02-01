using System;
using UnityEngine;

namespace Application_
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