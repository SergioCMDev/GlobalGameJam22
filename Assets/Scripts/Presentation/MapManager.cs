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
        public TileBase TileBase;
        public TileInnerData TileInnerData;
    }

    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private List<Vector3> world;
        [SerializeField] private List<TileBase> worldTile = new List<TileBase>();
        private Dictionary<Vector3, TileInnerData> innerDataFromTiles = new Dictionary<Vector3, TileInnerData>();
        [SerializeField] private List<TileTuple> innerTileDataFromTiles = new List<TileTuple>();


        private void Awake()
        {
            world = GetWorld();
            foreach (var tilePosition in world)
            {
                if (innerDataFromTiles.ContainsKey(tilePosition)) continue;
                innerDataFromTiles.Add(tilePosition, new TileInnerData());
            }

            foreach (var tileBase in worldTile)
            {
                innerTileDataFromTiles.Add(new TileTuple()
                {
                    TileBase = tileBase,
                    TileInnerData = new TileInnerData()
                });
            }
        }


        public TileInnerData GetTileData(Vector3Int tilePosition)
        {
            return !innerDataFromTiles.ContainsKey(tilePosition) ? null : innerDataFromTiles[tilePosition];
        }

        public void PlayerSetBuildingInTilemap(PlayerSetBuildingInTilemapEvent tilemapEvent)
        {
            var building = Instantiate(tilemapEvent.Prefab);
            tilemapEvent.SelectedTile.TileInnerData.Occupied = true;
            building.transform.position = _tilemap.GetCellCenterWorld(tilemapEvent.SelectedTile.GridPosition);
        }

        private List<Vector3> GetWorld()
        {
            var world = new List<Vector3>();
            for (int n = _tilemap.cellBounds.xMin; n < _tilemap.cellBounds.xMax; n++)
            {
                for (int p = _tilemap.cellBounds.yMin; p < _tilemap.cellBounds.yMax; p++)
                {
                    Vector3Int localPlace = (new Vector3Int(n, p, (int)_tilemap.transform.position.y));
                    Debug.Log($"TileMap Y Position{(int)_tilemap.transform.position.y}");
                    localPlace.z = 0;
                    Vector3 place = _tilemap.CellToWorld(localPlace);
                    if (!_tilemap.HasTile(localPlace)) continue;
                    var tile = _tilemap.GetTile(localPlace);
                    worldTile.Add(tile);
                    world.Add(place);
                }
            }

            // Debug.Log(world.ToString());
            return world;
        }

        public Tile GetTileBase(Vector3 inputMousePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(inputMousePosition);
            Vector3Int gridPosition = _tilemap.WorldToCell(mousePosition);
            var tile = _tilemap.GetTile<Tile>(gridPosition);
            return tile;
        }

        public Vector3Int GetGridPosition(Vector3 inputMousePosition)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = _tilemap.WorldToCell(mousePosition);
            return gridPosition;
        }

        public TileInnerData GetTileDataByTile(Tile tile)
        {
            return innerTileDataFromTiles.Any(x => x.TileBase == tile)
                ? null
                : innerTileDataFromTiles.Single(x => x.TileBase == tile).TileInnerData;
        }
    }
}