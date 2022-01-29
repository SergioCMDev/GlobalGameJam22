using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace IA
{
    public class GridPathfinding:MonoBehaviour
    {
        [SerializeField] 
        private Tilemap _tilemap;

        [SerializeField]
        private Enemy _enemy;

        public List<Vector3> availablePlaces;
        public List<Vector3> world;
        public Vector3 nextDestination;
        
        private void Awake()
        {
            _enemy.InitialPosition(_tilemap.cellBounds.max);
            GetWorld();
            nextDestination = Pathfinder(_enemy.transform.position);
        }

        private void Update()
        {
            _enemy.MoveTo(nextDestination);
            if (_enemy.transform.position == nextDestination)
            {
                nextDestination = Pathfinder(_enemy.transform.position);
            }
        }

        private Vector3Int Pathfinder(Vector3 currentPos)
        {
            foreach(var brick in availablePlaces)
            {
                HeuristicValue(currentPos, brick);
                nextDestination = brick;
                
            }
            availablePlaces.Remove(nextDestination);
            _enemy.MoveTo(_tilemap.cellBounds.max);
            return new Vector3Int();
        }

        public void GetWorld()
        {
            availablePlaces = new List<Vector3>();
            for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
            {
                for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int) _tilemap.transform.position.y));
                    Vector3 place = _tilemap.CellToWorld(localPlace);
                    if (_tilemap.HasTile(localPlace))
                    {
                        world.Add(place);
                        if (!_tilemap.GetTile(localPlace).name.Equals("tileGrass02"))
                        {
                            availablePlaces.Add(place);
                        }
                    }
                }
            }
            
            Debug.Log(world.ToString());
        }

        private void HeuristicValue(Vector3 current, Vector3 candidate)
        {
            
        }
    }
}