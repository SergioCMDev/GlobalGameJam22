﻿using System;
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

    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap, _tilemapOverWorld;
        [SerializeField] private List<TileTuple> innerTileDataFromTiles = new List<TileTuple>();
        [SerializeField] private List<Vector3> tilesToBlock = new List<Vector3>();
        [SerializeField] private SaveBuildingEvent saveBuildingEvent;

        public IDictionary<Vector3, Vector3Int> world;
        [SerializeField] private TileBase selectedTile;

        private void Awake()
        {
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
            if (!world.Any(X => X.Value == tilemapEvent.GridPosition)) return;
            var worldPosition = world.Single(X => X.Value == tilemapEvent.GridPosition).Key;
            var tileData = GetTileData(tilemapEvent.GridPosition);
            // var tileData2 = GetTileData(worldPosition);
            if (tileData.Occupied) return;

            var building = Instantiate(tilemapEvent.Prefab);
            tileData.Occupied = true;
            tileData.OccupiedBy = building;

            building.transform.position = _tilemap.GetCellCenterWorld(tilemapEvent.GridPosition);

            saveBuildingEvent.Instance = building;
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


        public Vector3Int GetGridPosition(Vector3 inputMousePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputMousePosition);
            Vector3Int gridPosition = _tilemapOverWorld.WorldToCell(mousePosition);
            return gridPosition;
        }


        private TileInnerData GetTileData(Vector3Int gridPosition)
        {
            return innerTileDataFromTiles.Single(x => x.GridPosition == gridPosition)
                .TileInnerData;
        }

        private TileInnerData GetTileData(Vector3 gridPosition)
        {
            return innerTileDataFromTiles.Single(x => x.WorlddPosition == gridPosition)
                .TileInnerData;
        }

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

        public void Occupy(Vector3Int gridPosition)
        {
            innerTileDataFromTiles.Single(x => x.GridPosition == gridPosition)
                .TileInnerData.Occupied = true;
        }

        public Tile GetTile(Vector3 mousePosition)
        {
            var gridPosition = GetGridPosition(mousePosition);
            var tile = _tilemapOverWorld.GetTile<Tile>(gridPosition);
            return tile;
        }

        // public Tile GetTileOverWorld(Vector3 mousePosition)
        // {
        //     var tile = _tilemapOverWorld.SetTile(gridPosition, selectedTile);
        //     return tile;
        // }

        public void SelectTTile(Vector3 mousePosition)
        {
            var gridPosition = GetGridPosition(mousePosition);
            _tilemapOverWorld.SetTile(gridPosition, selectedTile);
            _tilemapOverWorld.RefreshTile(gridPosition);
        }
    }
}