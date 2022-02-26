using System;
using System.Collections.Generic;
using System.Linq;
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

        private IDictionary<Vector3, Vector3Int> _yellowBrickRoad;
        private IDictionary<Vector3, Vector3Int> _defensiveBuilds;
        private IDictionary<Vector3, Vector3Int> _cityBuilds;

        private KeyValuePair<Vector3, Vector3Int> nextDestination;
        private KeyValuePair<Vector3, Vector3Int> lastBrick;
        private bool attacking = false; //mover a enemy
        public bool initialPotition = true; //mover a enemy

        private Vector3 start; //mover a map manager, usar tile especial
        private Vector3 end; //mover a map manager, usar tile especial
        private float currentTime = 0; //mover a enemy
        public float damage = 1f; //mover a enemy
        public float attackSpeed = 1;
    
    
        private void Start()
        {
            _defensiveBuilds = new Dictionary<Vector3, Vector3Int>();
            _yellowBrickRoad = new Dictionary<Vector3, Vector3Int>();
            _cityBuilds = new Dictionary<Vector3, Vector3Int>();
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
            if (enemy.transform.position == nextDestination.Key)
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
            enemy.EnemyMovement.MoveTo(nextDestination.Key);
        }
    
        private void DivideWorld()
        {
            foreach (var tile in gridBuildingManager.world)
            {
                switch (_tilemap.GetTile(tile.Value).name)
                {
                    case "bocetoedificos":
                        _cityBuilds.Add(tile);
                        break;
                    case "bocetotierra":
                        _yellowBrickRoad.Add(tile);
                        break;
                    case "bocetotorreta":
                        _defensiveBuilds.Add(tile);
                        break;
                }
            }
        }

        private KeyValuePair<Vector3, Vector3Int> Pathfinder(Vector3 currentPos)
        {
            KeyValuePair<Vector3, Vector3Int> bestOptionVectorToWorld = new KeyValuePair<Vector3, Vector3Int>();
            float bestOption = 99999;
        
            if (!_yellowBrickRoad.Any())
            {
                bestOptionVectorToWorld = lastBrick;
            }
            else
            {
                HeuristicValue(currentPos, _yellowBrickRoad, ref bestOption, ref bestOptionVectorToWorld);
                if(_defensiveBuilds.Any()) HeuristicValue(currentPos, _defensiveBuilds, ref bestOption, ref bestOptionVectorToWorld);
                HeuristicValue(currentPos, _cityBuilds, ref bestOption, ref bestOptionVectorToWorld);
            }

            if (_defensiveBuilds.Contains(bestOptionVectorToWorld))
            {
                attacking = true;
                lastBrick = nextDestination;
            }
            else if(_yellowBrickRoad.Contains(bestOptionVectorToWorld))
            {
                _yellowBrickRoad.Remove(bestOptionVectorToWorld);
            }
            /*else if (bestOptionVectorToWorld == end)
        {
            _defensiveBuilds.Clear();
            _yellowBrickRoad.Clear();
        }*/

            return bestOptionVectorToWorld;
        }

        private void DestroyTile(Building.Building obj)
        {
            if (_defensiveBuilds.Any())
            {
                var randomKey = _defensiveBuilds.Keys.ToArray()[(int)Random.Range(0, _defensiveBuilds.Keys.Count - 1)];
                _tilemap.SetTile(_defensiveBuilds[randomKey], cityDestroyed);
                _defensiveBuilds.Remove(randomKey);
            }
        
        }
        private void HeuristicValue(Vector3 current, IDictionary<Vector3, Vector3Int> listCandidates, ref float result, ref KeyValuePair<Vector3, Vector3Int> chosen)
        {
            float HValue;
            foreach(var tile in listCandidates)
            {
                HValue = Math.Abs(current.x - tile.Key.x) + Math.Abs(current.y - tile.Key.y);
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
