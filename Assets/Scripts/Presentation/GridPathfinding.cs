using System;
using System.Collections.Generic;
using System.Linq;
using App;
using Presentation.Building;
using Presentation.Hostiles;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Presentation
{
    public class GridPathfinding:MonoBehaviour
    {
        [SerializeField] private GridBuildingManager gridBuildingManager;
        [SerializeField] private Enemy enemy;
        [FormerlySerializedAs("_cityBuilding")] 
        [SerializeField] private CityBuilding cityBuilding;
    
    
        [SerializeField] private Tilemap _tilemap; //coger de mapManager
        [SerializeField] private bool friend; //mover a enemy
        [SerializeField] private Tile cityDestroyed; //mover a build

        private List<TileDataEntity> _yellowBrickRoad;
        private List<TileDataEntity> _defensiveBuilds;
        private List<TileDataEntity> _cityBuilds;

        private TileDataEntity nextDestination;
        private TileDataEntity lastBrick;
        private bool attacking = false; //mover a enemy
        public bool initialPotition = true; //mover a enemy

        private Vector3 start; //mover a map manager, usar tile especial
        private Vector3 end; //mover a map manager, usar tile especial
        private float currentTime = 0; //mover a enemy
        public float damage = 1f; //mover a enemy
        public float attackSpeed = 1;
    
    
        private void Start()
        {
            _defensiveBuilds = new List<TileDataEntity>();
            _yellowBrickRoad = new List<TileDataEntity>();
            _cityBuilds = new List<TileDataEntity>();
            DivideWorld();
        
        
            start = (friend) ? _tilemap.cellBounds.min : _tilemap.cellBounds.max; //mover a enemy
            end = (friend) ? _tilemap.cellBounds.max : _tilemap.cellBounds.min; //mover a enemy
            //_enemy.InitialPosition(_tilemap.WorldToLocal(start)); //mover a enemy
            nextDestination = Pathfinder(enemy.EnemyMovement.transform.position); //mover a enemy
            cityBuilding.OnBuildingDestroyed += DestroyTile; //mover a enemy

            initialPotition = false; //mover a enemy
        }

        private void Update()
        {
            if (enemy.transform.position == nextDestination.WorldPosition)
            {
                if (attacking)
                {
                    //attacking = true;
                    //nextDestination = lastBrick;
                    if (CanAttack() && cityBuilding.Life >= 0)
                    {
                        cityBuilding.ReceiveDamage(damage, _cityBuilds.Count);
                        currentTime -= attackSpeed;
                        Debug.Log("Damage percentage: " + (100/cityBuilding.MaxLife)*cityBuilding.Life);
                    }
                }
                else
                {
                    nextDestination = Pathfinder(enemy.transform.position);
                }
            }
            enemy.EnemyMovement.MoveTo(nextDestination.WorldPosition);
        }
    
        private void DivideWorld()
        {
            foreach (var tile in gridBuildingManager._worldTileDictionaryBuildingTilemap)
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

        private TileDataEntity Pathfinder(Vector3 currentPos)
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
                if(_defensiveBuilds.Any()) HeuristicValue(currentPos, _defensiveBuilds, ref bestOption, ref bestTileOption);
                HeuristicValue(currentPos, _cityBuilds, ref bestOption, ref bestTileOption);
            }

            if (_defensiveBuilds.Contains(bestTileOption))
            {
                attacking = true;
                lastBrick = nextDestination;
            }
            else if(_yellowBrickRoad.Contains(bestTileOption))
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

        private void DestroyTile(Building.Building obj)
        {
            if (_defensiveBuilds.Any())
            {
                //var randomKey = _defensiveBuilds.Keys.ToArray()[(int)Random.Range(0, _defensiveBuilds.Keys.Count - 1)];
                var randomKey = _defensiveBuilds[(int)Random.Range(0, _defensiveBuilds.Count - 1)];
                _tilemap.SetTile(randomKey.GridPosition, cityDestroyed);
                _defensiveBuilds.Remove(randomKey);
            }
        
        }
        private void HeuristicValue(Vector3 current, List<TileDataEntity> listCandidates, ref float result, ref TileDataEntity chosen)
        {
            float HValue;
            foreach(var tile in listCandidates)
            {
                HValue = Math.Abs(current.x - tile.WorldPosition.x) + Math.Abs(current.y - tile.WorldPosition.y);
                if (HValue < result)
                {
                    chosen = tile;
                    result = HValue;
                };
            }
        }
    
        private bool CanAttack()
        {
            currentTime += Time.deltaTime;
            return currentTime > attackSpeed;
        }
    
    
    
    }
}
