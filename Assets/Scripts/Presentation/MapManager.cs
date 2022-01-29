using System.Collections.Generic;
using Application_;
using Application_.Events;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Presentation
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _map;
        private Dictionary<TileBase, TileInnerData> innerDataFromTiles = new Dictionary<TileBase, TileInnerData>();


        private void Awake()
        {

            foreach (var tile in _map.GetTilesBlock(_map.cellBounds))
            {
                innerDataFromTiles.Add(tile, new TileInnerData());
            }
        }


        public TileInnerData GetTileData(Vector3Int tilePosition)
        {
            TileBase tile = _map.GetTile(tilePosition);

            return tile == null ? null : innerDataFromTiles[tile];
        }

        public void PlayerSetBuildingInTilemap(PlayerSetBuildingInTilemapEvent tilemapEvent)
        {
            var building = Instantiate(tilemapEvent.Prefab);
            tilemapEvent.SelectedTile.TileData.Occupied = true;
            building.transform.position = _map.GetCellCenterWorld(tilemapEvent.SelectedTile.GridPosition);
        }
    }
}