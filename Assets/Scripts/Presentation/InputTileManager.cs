using System;
using System.Collections;
using Application_;
using UnityEngine;


namespace Presentation
{
    public class InputTileManager : MonoBehaviour
    {
        [SerializeField] private MapManager mapManager;

        private bool _playerCanSelectTile;

        private IEnumerator selectTileByClick;
        // public event Action<SelectedTileData> OnPlayerHasSelectedTile;

        //TODO CHECK IF WITH ONMOUSEDOWN WORKS
        private IEnumerator SelectTileByClick(Action CancelSelectionOfTile,
            Action<SelectedTileData> OnPlayerHasSelectedTile, Action SelectedTileIsOccupied)
        {
            Debug.Log("START SELECTION");
            while (_playerCanSelectTile)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("Canceled Buy");
                    CancelSelectionOfTile.Invoke();
                    _playerCanSelectTile = false;
                    yield break;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3Int gridPosition = mapManager.GetGridPosition(Input.mousePosition);
                    Debug.Log($"SELECTED {gridPosition} CLICK {Input.anyKey}");

                    if (mapManager.IsOccupied(gridPosition))
                    {
                        SelectedTileIsOccupied.Invoke();
                        yield break;
                    }
                    
                    if (!mapManager.CanBeUsed(gridPosition))
                    {
                        yield break;
                    }

                    mapManager.Occupy(gridPosition);
                    OnPlayerHasSelectedTile?.Invoke(new SelectedTileData
                    {
                        GridPosition = gridPosition,
                        // TileInnerData = data
                    });
                    _playerCanSelectTile = false;
                    yield break;
                }

                Debug.Log("WAITING");
                yield return null;
            }
        }

        public void EnableTileSelection(Action cancelBuy, Action<SelectedTileData> playerHasSelectedTile,
            Action tileIsOccupied)
        {
            _playerCanSelectTile = true;
            if (selectTileByClick != null)
            {
                StopCoroutine(selectTileByClick);
            }

            selectTileByClick = SelectTileByClick(cancelBuy, playerHasSelectedTile, tileIsOccupied);
            StartCoroutine(selectTileByClick);
        }
    }
}