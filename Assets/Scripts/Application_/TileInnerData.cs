using System;

namespace Application_
{
    [Serializable]
    public class TileInnerData
    {
        public bool Occupied;
        public TileInnerData()
        {
            Occupied = false;
        }
    }
}