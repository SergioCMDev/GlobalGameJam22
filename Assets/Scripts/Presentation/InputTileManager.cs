using System;
using System.Collections;
using Application_;
using UnityEngine;


namespace Presentation
{
    public class InputTileManager : MonoBehaviour
    {
        [SerializeField] private GridBuildingManager gridBuildingManager;
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
                Vector3Int gridPosition = gridBuildingManager.GetGridPositionByMouse(Input.mousePosition);

                if(gridBuildingManager.PositionExists(gridPosition))
                    // MarkSelectedTile();
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

                    if ( !gridBuildingManager.PositionExists(gridPosition) || gridBuildingManager.IsOccupied(gridPosition) || !gridBuildingManager.CanBeUsed(gridPosition))
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

        // private void MarkSelectedTile()
        // {
        //     // var tile = mapManager.GetTileOverWorld(Input.mousePosition);
        //     gridBuildingManager.SelectTTile(Input.mousePosition);
        // }
        //
        // private void MarkDeselectedTile()
        // {
        //     // var tile = mapManager.GetTileOverWorld(Input.mousePosition);
        //     gridBuildingManager.DeselectTTile(Input.mousePosition);
        // }

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