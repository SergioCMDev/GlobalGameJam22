using System.Collections.Generic;
using App.Info.Enemies;
using App.Info.Paths;
using App.Info.Tiles;
using UnityEngine;

namespace Services.PathRetriever
{
    [CreateAssetMenu(fileName = "PathRetrieverService",
        menuName = "Loadable/Services/PathRetrieverService")]
    public class PathRetrieverService : LoadableComponent
    {
        [SerializeField] private List<PathLevelInfo> pathInfo;

        public List<TilePosition> GetTilesToFollowByEnemyAndLevel(EnemyType enemyType, string level)
        {
            foreach (var info in pathInfo)
            {
                if (info.enemyType == enemyType && info.level == level)
                {
                    return info.TilesToFollow;
                }
            }

            return null;
        }
        public override void Execute()
        {
            Debug.Log("Execute PathRetrieverService");
        }
    }
}