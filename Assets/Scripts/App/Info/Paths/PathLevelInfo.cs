using System;
using System.Collections.Generic;
using App.Info.Enemies;
using App.Info.Tiles;

namespace App.Info.Paths
{
    [Serializable]
    public class PathLevelInfo
    {
        public TilesToFollow tileToFollow;
        public EnemyType enemyType;
        public string level;

        public List<TilePosition> TilesToFollow => tileToFollow.tilePositions;
    }
}