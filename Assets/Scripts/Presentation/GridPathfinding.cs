using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace IA
{
    public class GridPathfinding:MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private EnemyMovement enemyMovement;
        [SerializeField] private bool friend;

        [FormerlySerializedAs("availablePlaces")] public List<Vector3> yellowBrickRoad;
        public List<Vector3> world;
        public List<Vector3> defensiveBuilds;

        public Vector3 nextDestination;
        public Vector3 lastBrick;
        public bool attacking = false;
        public bool initialPotition = true;

        public Vector3 start;
        public Vector3 end;
        
        
        private void Awake()
        {
            start = (friend) ? _tilemap.cellBounds.min : _tilemap.cellBounds.max;
            end = (friend) ? _tilemap.cellBounds.max : _tilemap.cellBounds.min;
            //_enemy.InitialPosition(_tilemap.WorldToLocal(start));
            GetWorld();
            nextDestination = Pathfinder(enemyMovement.transform.position);
            initialPotition = false;
        }

        private void Update()
        {
            if (enemyMovement.transform.position == nextDestination)
            {
                if (attacking)
                {
                    attacking = true;
                    //nextDestination = lastBrick;
                }
                else
                {
                    nextDestination = Pathfinder(enemyMovement.transform.position);
                }
            }
            //TODO CHANGE TO ENEMY
            enemyMovement.MoveTo(nextDestination);
        }

        private Vector3 Pathfinder(Vector3 currentPos)
        {
            Vector3 bestOptionVector = new Vector3();
            float bestOption = 99999;
            
            //activar si se quiere atacar a los edificios
            if (!yellowBrickRoad.Any() && !defensiveBuilds.Any())
            //if (!yellowBrickRoad.Any())
            {
                bestOptionVector = end;
            }
            else
            {
                HeuristicValue(currentPos, yellowBrickRoad, ref bestOption,  ref bestOptionVector);
                //activar si se quiere atacar a los edificios
                HeuristicValue(currentPos, defensiveBuilds, ref bestOption, ref bestOptionVector);
            }

            if (defensiveBuilds.Contains(bestOptionVector))
            {
                defensiveBuilds.Remove(bestOptionVector);
                attacking = true;
                lastBrick = nextDestination;
                
                //Quitar si implementado el ataque a torretas
                bestOptionVector = nextDestination;
            }
            else if(yellowBrickRoad.Contains(bestOptionVector))
            {
                yellowBrickRoad.Remove(bestOptionVector);
                attacking = false;
            }else if (bestOptionVector == end)
            {
                defensiveBuilds.Clear();
                yellowBrickRoad.Clear();
            }

            return bestOptionVector;
        }

        public void GetWorld()
        {
            yellowBrickRoad = new List<Vector3>();
            defensiveBuilds = new List<Vector3>();
            for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
            {
                for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int) _tilemap.transform.position.y));
                    Vector3 place = _tilemap.CellToWorld(localPlace);
                    if (_tilemap.HasTile(localPlace))
                    {
                        world.Add(place);
                        if (_tilemap.GetTile(localPlace).name.Equals("bocetoedificos"))
                        {
                            defensiveBuilds.Add(place);
                        } 
                        else if (_tilemap.GetTile(localPlace).name.Equals("bocetotierra"))
                        {
                            yellowBrickRoad.Add(place);
                        }
                    }
                }
            }
        }

        private void HeuristicValue(Vector3 current, List<Vector3> ListCandidates, ref float result, ref Vector3 chosen)
        {
            float HValue;
            
            foreach(var tile in ListCandidates)
            {
                HValue = Math.Abs(current.x - tile.x) + Math.Abs(current.y - tile.y);
                if (HValue < result)
                {
                    chosen = tile;
                    result = HValue;
                };
            }
        }
        
    }
}