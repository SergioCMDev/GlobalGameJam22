using System;
using System.Collections.Generic;
using App.Info.Tiles;
using UnityEngine;

namespace App.Info.Enemies
{
    [Serializable]
    public class EnemySpawnerInfo
    {
        public EnemyInfo enemyInfo;
        public EnemyType enemyType = EnemyType.Normal;
        public GameObject enemyPrefab;
        public TilesToFollow tileToFollow;

        public List<TilePosition> TilesToFollow => tileToFollow.tilePositions;
    }

    public enum EnemyType
    {
        Normal, Red
    }
}