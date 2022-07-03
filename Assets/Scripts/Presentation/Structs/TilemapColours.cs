using System;

namespace Presentation.Structs
{
    [Serializable]
    public struct TilemapColours
    {
        public TileType OriginalColour;
        public TileType PreviousColour;
        public TileType CurrentColour;

        public TilemapColours(TileType currentColour)
        {
            OriginalColour = currentColour;
            PreviousColour = currentColour;
            CurrentColour = currentColour;
        }

        public void Clear()
        {
            OriginalColour = TileType.Empty;
            PreviousColour = TileType.Empty;
            CurrentColour = TileType.Empty;
        }
    }
}