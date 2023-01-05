using System.Collections.Generic;
using Services.EnemySpawner;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Services.TileReader
{
    [CreateAssetMenu(fileName = "TileReaderService",
        menuName = "Loadable/Services/TileReaderService")]
    public class TileReaderService : LoadableComponent
    {
        [SerializeField] private List<TileTupleData> tileTupleData;

        public Tile GetTileByColor(TileColor enemyType)
        {
            foreach (var tupleData in tileTupleData)
            {
                if (tupleData.TileColor == enemyType)
                {
                    return tupleData.tile;
                }
            }

            return null;
        }
        public override void Execute()
        {
        }
    }
}