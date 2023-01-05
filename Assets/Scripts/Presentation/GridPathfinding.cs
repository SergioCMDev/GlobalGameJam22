using System.Collections.Generic;
using App;
using Presentation.Managers;
using UnityEngine;

namespace Presentation
{
    public class GridPathfinding
    {
        private List<TileDataEntity> _yellowBrickRoad;
        private List<TileDataEntity> _defensiveBuilds;
        private List<TileDataEntity> _cityBuilds;

        private TileDataEntity nextDestination;
        private TileDataEntity lastBrick;
        private Dictionary<Vector3Int, TileDataEntity> _worldTileDictionaryBuildingTilemap;
        public TilePosition LastPosition { get; set; }
        public TilePosition InitialTile { get; set; }

        private List<TilePosition> positionsToFollow = new();

        public void Init(Dictionary<Vector3Int, TileDataEntity> worldDictionary, List<TilePosition> positions)
        {
            Init(worldDictionary);

            foreach (var positionToClone in positions)
            {
                positionsToFollow.Add(new TilePosition()
                {
                    GridPosition = positionToClone.GridPosition
                });
            }

            foreach (var position in positionsToFollow)
            {
                position.WorldPosition = worldDictionary[position.GridPosition].WorldPosition;
            }

            InitialTile = positionsToFollow[0];
            LastPosition = positionsToFollow[^1];
        }

        public void Init(Dictionary<Vector3Int, TileDataEntity> worldDictionary)
        {
            _defensiveBuilds = new List<TileDataEntity>();
            _yellowBrickRoad = new List<TileDataEntity>();
            _cityBuilds = new List<TileDataEntity>();
            _worldTileDictionaryBuildingTilemap = worldDictionary;
            DivideWorld();
        }


        private void DivideWorld()
        {
            foreach (var tile in _worldTileDictionaryBuildingTilemap)
            {
                switch (tile.Value.TileBaseWorld.name)
                {
                    case "bocetoedificos":
                        _cityBuilds.Add(tile.Value);
                        break;
                    case "bocetotierra":
                        _yellowBrickRoad.Add(tile.Value);
                        break;
                    case "bocetotorreta":
                        _defensiveBuilds.Add(tile.Value);
                        break;
                }
            }
        }

        // public TileDataEntity GetNextPositionFromCurrent(Vector3 currentPos)
        // {
        //     TileDataEntity bestTileOption = new TileDataEntity();
        //     float bestOption = 99999;
        //
        //     if (!_yellowBrickRoad.Any())
        //     {
        //         bestTileOption = lastBrick;
        //     }
        //     else
        //     {
        //         HeuristicValue(currentPos, _yellowBrickRoad, ref bestOption, ref bestTileOption);
        //         if (_defensiveBuilds.Any())
        //             HeuristicValue(currentPos, _defensiveBuilds, ref bestOption, ref bestTileOption);
        //         HeuristicValue(currentPos, _cityBuilds, ref bestOption, ref bestTileOption);
        //     }
        //
        //     if (_defensiveBuilds.Contains(bestTileOption))
        //     {
        //         lastBrick = nextDestination;
        //     }
        //     else if (_yellowBrickRoad.Contains(bestTileOption))
        //     {
        //         _yellowBrickRoad.Remove(bestTileOption);
        //     }
        //     /*else if (bestOptionVectorToWorld == end)
        // {
        //     _defensiveBuilds.Clear();
        //     _yellowBrickRoad.Clear();
        // }*/
        //
        //     return bestTileOption;
        // }
        //
        //
        // private void HeuristicValue(Vector3 current, List<TileDataEntity> listCandidates, ref float result,
        //     ref TileDataEntity chosen)
        // {
        //     float HValue;
        //     foreach (var tile in listCandidates)
        //     {
        //         HValue = Math.Abs(current.x - tile.WorldPosition.x) + Math.Abs(current.y - tile.WorldPosition.y);
        //         if (HValue < result)
        //         {
        //             chosen = tile;
        //             result = HValue;
        //         }
        //     }
        // }

        public TilePosition GetNextPositionFromCurrent(TilePosition enemyMovementTilePosition)
        {
            var positionToFollowNext = new TilePosition();
            for (int i = 0; i < positionsToFollow.Count; i++)
            {
                if (positionsToFollow[i].GridPosition != enemyMovementTilePosition.GridPosition) continue;
                positionToFollowNext = positionsToFollow[i + 1];
                break;
            }

            return positionToFollowNext;
        }
    }
}