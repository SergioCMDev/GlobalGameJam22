using System;
using System.Collections.Generic;
using System.Linq;
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

        public TileDataEntity GetNextPositionFromCurrent(Vector3 currentPos)
        {
            TileDataEntity bestTileOption = new TileDataEntity();
            float bestOption = 99999;

            if (!_yellowBrickRoad.Any())
            {
                bestTileOption = lastBrick;
            }
            else
            {
                HeuristicValue(currentPos, _yellowBrickRoad, ref bestOption, ref bestTileOption);
                if (_defensiveBuilds.Any())
                    HeuristicValue(currentPos, _defensiveBuilds, ref bestOption, ref bestTileOption);
                HeuristicValue(currentPos, _cityBuilds, ref bestOption, ref bestTileOption);
            }

            if (_defensiveBuilds.Contains(bestTileOption))
            {
                lastBrick = nextDestination;
            }
            else if (_yellowBrickRoad.Contains(bestTileOption))
            {
                _yellowBrickRoad.Remove(bestTileOption);
            }
            /*else if (bestOptionVectorToWorld == end)
        {
            _defensiveBuilds.Clear();
            _yellowBrickRoad.Clear();
        }*/

            return bestTileOption;
        }


        private void HeuristicValue(Vector3 current, List<TileDataEntity> listCandidates, ref float result,
            ref TileDataEntity chosen)
        {
            float HValue;
            foreach (var tile in listCandidates)
            {
                HValue = Math.Abs(current.x - tile.WorldPosition.x) + Math.Abs(current.y - tile.WorldPosition.y);
                if (HValue < result)
                {
                    chosen = tile;
                    result = HValue;
                }
            }
        }
    }
}