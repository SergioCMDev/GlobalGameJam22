using System;
using System.Collections;
using System.Collections.Generic;
using Application_;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Presentation
{
    public class InputTileManager : MonoBehaviour
    {
        // [SerializeField] private Tilemap map;
        [SerializeField] private MapManager mapManager;
        
        private bool _playerCanSelectTile;
        // private List<Vector3Int> activeFires = new List<Vector3Int>();
        public event Action<SelectedTileData> OnPlayerHasSelectedTile;

        // private void AddBuildingOnTile(Vector3Int tilePosition, GameObject buildingPrefab)
        // {
        //     var newFire = Instantiate(buildingPrefab);
        //     newFire.transform.position = map.GetCellCenterWorld(tilePosition);
        //     // newFire.StartBurning(tilePosition, data, this);
        //
        //     activeFires.Add(tilePosition);
        // }

        private IEnumerator SelectTileByClick(Action CancelSelectionOfTile,
            Action<SelectedTileData> OnPlayerHasSelectedTile, Action SelectedTileIsOccupied)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("START SELECTION");
            _playerCanSelectTile = true;
            while (_playerCanSelectTile)
            {
                if (!Input.GetMouseButtonDown(0)) yield return null;
                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("Canceled Buy");
                    CancelSelectionOfTile.Invoke();
                    _playerCanSelectTile = false;
                    yield break;
                }

                // mapManager.GetTileBase(Input.mousePosition);
                // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // Vector3Int gridPosition = map.WorldToCell(mousePosition);
                Vector3Int gridPosition = mapManager.GetGridPosition(Input.mousePosition);
                Tile tile = mapManager.GetTileBase(Input.mousePosition);
                Debug.Log($"SELECTED {gridPosition} TILE {tile}");

                var data = mapManager.GetTileData(gridPosition);
                var dataTile = mapManager.GetTileDataByTile(tile);
                if (data == null) yield break;
                if (data.Occupied)
                {
                    SelectedTileIsOccupied.Invoke();
                    yield break;
                }

                OnPlayerHasSelectedTile?.Invoke(new SelectedTileData
                {
                    GridPosition = gridPosition,
                    TileInnerData = data
                });
                _playerCanSelectTile = false;
                yield break;
            }
        }

        // private void Update()
        // {
        //     // if (!_playerCanSelectTile) return;
        //     // if (!Input.GetMouseButtonDown(0)) return;
        //     // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     // Vector3Int gridPosition = map.WorldToCell(mousePosition);
        //     // TileBase tile = map.GetTile(gridPosition);
        //     //
        //     // TileData data = mapManager.GetTileData(gridPosition);
        //     // if (!data.Occupied)
        //     //     _playerCanSelectTile = false;
        //     // AddBuildingOnTile(gridPosition, data);
        // }

        public void EnableTileSelection(Action cancelBuy, Action<SelectedTileData> OnPlayerHasSelectedTile,
            Action OnTileOccupied)
        {
            StartCoroutine(SelectTileByClick(cancelBuy, OnPlayerHasSelectedTile, OnTileOccupied));
        }
    }
}