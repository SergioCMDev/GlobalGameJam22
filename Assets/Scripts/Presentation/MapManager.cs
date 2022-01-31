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
        public Tile TileBase;
        public TileInnerData TileInnerData;
    }

    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        public IDictionary<Vector3, Vector3Int> world;
        [SerializeField] private List<Vector3> occupiedWorld = new List<Vector3>();
        [SerializeField] private List<Tile> worldTile = new List<Tile>();
        [SerializeField] private Dictionary<Vector3, TileInnerData> innerDataFromTiles = new Dictionary<Vector3, TileInnerData>();
        [SerializeField] private List<TileTuple> innerTileDataFromTiles = new List<TileTuple>();
        [SerializeField] private SaveBuildingEvent saveBuildingEvent;


        private void Awake()
        {
            GetWorld();
            foreach (var tilePosition in world.Keys)
            {
                innerDataFromTiles.Add(tilePosition, new TileInnerData());
            }

            // foreach (var tileBase in worldTile)
            // {
            //     innerTileDataFromTiles.Add(new TileTuple()
            //     {
            //         TileBase = tileBase,
            //         TileInnerData = new TileInnerData()
            //     });
            // }
        }


        // public TileInnerData GetTileData(Vector3Int tilePosition)
        // {
        //     return _tilemap.GetTile<BuildableTile>(tilePosition).tileInnerData;
        // }

        public void PlayerSetBuildingInTilemap(PlayerSetBuildingInTilemapEvent tilemapEvent)
        {
            var building = Instantiate(tilemapEvent.Prefab);
            building.transform.position = _tilemap.GetCellCenterWorld(tilemapEvent.SelectedTile.GridPosition);
            saveBuildingEvent.Instance = building;
            saveBuildingEvent.Fire();
        }

        private void GetWorld()
        {
            world = new Dictionary<Vector3, Vector3Int>();
            for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
            {
                for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int) _tilemap.transform.position.y));
                    Vector3 place = _tilemap.CellToWorld(localPlace);
                    if (_tilemap.HasTile(localPlace))
                    {
                        world.Add(place, localPlace);
                    }
                }
            }
        }

        public T GetTile<T>(Vector3 inputMousePosition) where T :TileBase
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputMousePosition);
            Vector3Int gridPosition = _tilemap.WorldToCell(mousePosition);
            var tile = _tilemap.GetTile<T>(gridPosition);
            return tile;
        }

        public Vector3Int GetGridPosition(Vector3 inputMousePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputMousePosition);
            Vector3Int gridPosition = _tilemap.WorldToCell(mousePosition);
            return gridPosition;
        }

        public TileInnerData GetTileDataByTile(Tile tile)
        {
            return innerTileDataFromTiles.Any(x => x.TileBase == tile)
                ? null
                : innerTileDataFromTiles.Single(x => x.TileBase == tile).TileInnerData;
        }

        // public bool IsOccupied(BuildableTile tile)
        // {
        //     return tile.IsOccupied();
        //
        // }
        
        public bool IsOccupied(Vector3 gridPosition)
        {
             return occupiedWorld.Contains(gridPosition);
        }

        public void Occupy(Vector3Int gridPosition)
        {
            occupiedWorld.Add(gridPosition);
        }

        public bool CanBeUsed(Vector3Int gridPosition)
        {
            return _tilemap.HasTile(gridPosition);
        }
    }
}