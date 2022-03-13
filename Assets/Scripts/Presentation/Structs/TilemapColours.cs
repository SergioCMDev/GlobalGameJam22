using System;

namespace Presentation
{
    [Serializable]
    public struct TilemapColours
    {
        public TileType OriginalColour;
        public TileType PreviousColour;
        public TileType CurrentColour;
    }
}