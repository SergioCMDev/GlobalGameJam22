using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace IA
{
    public class GridPathfinding:MonoBehaviour
    {
        [SerializeField] 
        private Tilemap _tilemap;

        [SerializeField]
        private Enemy _enemy;

        [FormerlySerializedAs("availablePlaces")] public List<Vector3> yellowBrickRoad;
        public List<Vector3> world;
        public Vector3 nextDestination;
        
        private void Awake()
        {
            _enemy.InitialPosition(_tilemap.cellBounds.max);
            GetWorld();
        }

        private void Update()
        {
            _enemy.MoveTo(nextDestination);
            if (_enemy.transform.position == nextDestination)
            {
                nextDestination = Pathfinder(_enemy.transform.position);
            }
        }

        private Vector3 Pathfinder(Vector3 currentPos)
        {
            Vector3 aux = new Vector3();
            float bestOption = 99999;
            float HValue = 0;

            if (!yellowBrickRoad.Any())
            {
                aux =_tilemap.cellBounds.min;
            }
            
            foreach(var brick in yellowBrickRoad)
            {
                HValue = HeuristicValue(currentPos, brick);
                if (HValue < bestOption)
                {
                    aux = brick;
                    bestOption = HValue;
                };
            }
            yellowBrickRoad.Remove(aux);

            return aux;
        }

        public void GetWorld()
        {
            yellowBrickRoad = new List<Vector3>();
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
                            yellowBrickRoad.Add(place);
                        }
                    }
                }
            }
        }

        private float HeuristicValue(Vector3 current, Vector3 candidate)
        {
            return Vector3.Distance(current, candidate);
        }
    }
}