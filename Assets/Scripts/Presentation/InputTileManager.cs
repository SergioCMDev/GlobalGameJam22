using System;
using System.Collections;
using Application_;
using UnityEngine;


namespace Presentation
{
    public class InputTileManager : MonoBehaviour
    {
        [SerializeField] private MapManager mapManager;
        [SerializeField] private Sprite hoverTile;

        private bool _playerCanSelectTile;

        private Coroutine selectTileByClick;
        // public event Action<SelectedTileData> OnPlayerHasSelectedTile;

        //TODO CHECK IF WITH ONMOUSEDOWN WORKS
        private IEnumerator SelectTileByClick(Action CancelSelectionOfTile,
            Action<SelectedTileData> OnPlayerHasSelectedTile, Action SelectedTileIsOccupied)
        {
            Debug.Log("START SELECTION");
            while (_playerCanSelectTile)
            {
                Vector3Int gridPosition = mapManager.GetGridPositionByMouse(Input.mousePosition);

                if(mapManager.PositionExists(gridPosition))
                    MarkSelectedTile();
                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("Canceled Buy");
                    CancelSelectionOfTile.Invoke();
                    _playerCanSelectTile = false;
                    yield break;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log($"SELECTED {gridPosition} CLICK {Input.anyKey}");

                    if ( !mapManager.PositionExists(gridPosition) || mapManager.IsOccupied(gridPosition) || !mapManager.CanBeUsed(gridPosition))
                    {
                        SelectedTileIsOccupied.Invoke();
                        yield break;
                    }
                    
                    OnPlayerHasSelectedTile?.Invoke(new SelectedTileData
                    {
                        GridPosition = gridPosition,
                    });
                    
                    _playerCanSelectTile = false;
                    yield break;
                }

                Debug.Log("WAITING");
                yield return null;
            }
        }

        private void MarkSelectedTile()
        {
            // var tile = mapManager.GetTileOverWorld(Input.mousePosition);
            mapManager.SelectTTile(Input.mousePosition);
        }
        
        private void MarkDeselectedTile()
        {
            // var tile = mapManager.GetTileOverWorld(Input.mousePosition);
            mapManager.DeselectTTile(Input.mousePosition);
        }

        public void EnableTileSelection(Action cancelBuy, Action<SelectedTileData> playerHasSelectedTile,
            Action tileIsOccupied)
        {
            _playerCanSelectTile = true;
            if (selectTileByClick != null)
            {
                StopCoroutine(selectTileByClick);
            }

            selectTileByClick = StartCoroutine(SelectTileByClick(cancelBuy, playerHasSelectedTile, tileIsOccupied));
        }
    }
}