using System;
using System.Collections.Generic;
using App;
using App.Info.Enemies;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Services.EnemySpawner
{
    [CreateAssetMenu(fileName = "EnemySpawnerService",
        menuName = "Loadable/Services/EnemySpawnerService")]
    public class EnemySpawnerService : LoadableComponent
    {
        [SerializeField] private List<EnemySpawnerInfo> enemyPrefabs;

        public EnemySpawnerInfo GetEnemyPrefabByType(EnemyType enemyType)
        {
            foreach (var enemySpawnerInfo in enemyPrefabs)
            {
                if (enemySpawnerInfo.enemyType == enemyType)
                {
                    return enemySpawnerInfo;
                }
            }

            return null;
        }
        public override void Execute()
        {
        }
    }

    [Serializable]
    public class TileTupleData
    {
        public TileColor TileColor;
        public Tile tile;
    }

    public enum TileColor
    {
        Red, White, Green, Blue, Purple, Empty
    }
}