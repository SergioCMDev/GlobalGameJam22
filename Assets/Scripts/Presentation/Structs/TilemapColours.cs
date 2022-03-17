using System;

namespace Presentation.Structs
{
    [Serializable]
    public struct TilemapColours
    {
        public TileType OriginalColour;
        public TileType PreviousColour;
        public TileType CurrentColour;
    }
}