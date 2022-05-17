using System;
using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Managers
{
    [Serializable]
    public class EnemySpawnerInfo
    {
        public EnemyInfo enemyInfo;
        public GameObject enemyPrefab;
        public TilesToFollow tileToFollow;

        public List<TilePosition> TilesToFollow => tileToFollow.tilePositions;
    }
}