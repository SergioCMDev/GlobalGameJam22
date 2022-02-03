using System;
using System.Collections.Generic;
using System.Linq;
using Application_;
using Application_.Events;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Presentation
{
    [Serializable]
    public struct TileTuple
    {
        public Vector3 WorlddPosition;
        public Vector3Int GridPosition;
        public TileInnerData TileInnerData;
    }

    public class GridBuildingManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap, _tilemapOverWorld;
        [SerializeField] private Grid _grid;

        [SerializeField] private List<TileTuple> innerTileDataFromTiles = new List<TileTuple>();
        [SerializeField] private List<Vector3> tilesToBlock = new List<Vector3>();
        [SerializeField] private SaveBuildingEvent saveBuildingEvent;
        [SerializeField] private Tile _selectedTile, _deselectedTile, _red, white, green;

        private Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

        public IDictionary<Vector3, Vector3Int> world;
        private GameObject _building;
        private BoundsInt _temporalArea;

        private void Awake()
        {
            string tilePath = @"Tiles\";
            tileBases.Add(TileType.Empty, null);
            tileBases.Add(TileType.White, white);
            // tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
            tileBases.Add(TileType.Green, green);
            // tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
            // tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
            tileBases.Add(TileType.Red, _red);


            _tilemapOverWorld.gameObject.SetActive(false);
            ReadWorld();
            foreach (var tilePosition in world.Keys)
            {
                innerTileDataFromTiles.Add(new TileTuple()
                {
                    GridPosition = world[tilePosition],
                    WorlddPosition = tilePosition,
                    TileInnerData = new TileInnerData()
                });
            }

            foreach (var tileToBlock in tilesToBlock)
            {
                var gridPosition = world[tileToBlock];
                if (innerTileDataFromTiles.All(x => x.GridPosition != gridPosition)) continue;
                innerTileDataFromTiles.Single(x => x.GridPosition != gridPosition).TileInnerData.CanBeUsed = false;
            }
        }


        public void PlayerSetBuildingInTilemap(PlayerSetBuildingInTilemapEvent tilemapEvent)
        {
            ShowTemporalTileMap();
            _building = Instantiate(tilemapEvent.Prefab);
            _temporalArea = _building.GetComponent<Building>().Area;

            _building.transform.position = _tilemap.GetCellCenterWorld(new Vector3Int());
            saveBuildingEvent.Instance = _building;
            saveBuildingEvent.Fire();
        }
        

        private void ReadWorld()
        {
            world = new Dictionary<Vector3, Vector3Int>();
            for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
            {
                for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int)_tilemap.transform.position.y));
                    Vector3 place = _tilemap.CellToWorld(localPlace);
                    if (_tilemap.HasTile(localPlace))
                    {
                        world.Add(place, localPlace);
                    }
                }
            }
        }


        public Vector3Int GetGridPositionByMouse(Vector3 inputMousePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputMousePosition);
            Vector3Int gridPosition = _grid.LocalToCell(mousePosition);
            // Vector3Int gridPosition = _grid.WorldToCell(mousePosition);
            return gridPosition;
        }


        private TileInnerData GetTileData(Vector3Int gridPosition)
        {
            return innerTileDataFromTiles.Single(x => x.GridPosition == gridPosition)
                .TileInnerData;
        }

        // private TileInnerData GetTileData(Vector3 gridPosition)
        // {
        //     return innerTileDataFromTiles.Single(x => x.WorlddPosition == gridPosition)
        //         .TileInnerData;
        // }

        public bool PositionExists(Vector3 gridPosition)
        {
            return innerTileDataFromTiles.Any(x => x.GridPosition == gridPosition);
        }

        public bool IsOccupied(Vector3 gridPosition)
        {
            return innerTileDataFromTiles.Single(x => x.GridPosition == gridPosition)
                .TileInnerData.Occupied;
        }


        public bool CanBeUsed(Vector3Int gridPosition)
        {
            return GetTileData(gridPosition).CanBeUsed;
        }

        // public void Occupy(Vector3Int gridPosition)
        // {
        //     innerTileDataFromTiles.Single(x => x.GridPosition == gridPosition)
        //         .TileInnerData.Occupied = true;
        // }
        
        public void ShowTemporalTileMap()
        {
            _tilemapOverWorld.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!_tilemapOverWorld.gameObject.activeInHierarchy || !_building) return;
            var gridPosition = GetGridPositionByMouse(Input.mousePosition);
            _building.transform.position = _tilemap.GetCellCenterLocal(gridPosition);
            var tiles = new TileBase[_temporalArea.size.x * _temporalArea.size.y * _temporalArea.size.z];

            var filledTiles = FillTiles(tiles, TileType.Green);
            var buildingArea = GetBuildingArea(gridPosition, _temporalArea.size);
            SetTilesInTilemap(buildingArea, filledTiles, _tilemapOverWorld);
            
            if (Input.GetMouseButtonDown(0))
            {
 
            // saveBuildingEvent.Instance = _building;
            // saveBuildingEvent.Fire();
            }
            
            if (Input.GetMouseButtonDown(1))
            {
    
            }
        }

        private BoundsInt GetBuildingArea(Vector3Int gridPosition, Vector3Int sizeArea)
        {
            BoundsInt buildingArea = new BoundsInt(gridPosition, sizeArea);
            return buildingArea;
        }

        private void SetTilesInTilemap(BoundsInt buildingArea, TileBase[] tileArray, Tilemap tilemap)
        {
            tilemap.SetTilesBlock(buildingArea, tileArray);
        }

        private TileBase[] FillTiles(TileBase[] tileArray, TileType type)
        {
            for (int i = 0; i < tileArray.ToArray().Length; i++)
            {
                tileArray[i] = tileBases[type];
            }

            return tileArray;
        }
    }
}