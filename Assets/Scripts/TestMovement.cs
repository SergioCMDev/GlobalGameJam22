using App.Events;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField] private ObjectHasMovedToNewTileEvent _eventMovement;
    [SerializeField] private Grid _grid;
    private Vector3Int buildingGridPosition, _currentObjectPosition, _previousObjectPosition;
    
    

    // Update is called once per frame
    void Update()
    {
        _currentObjectPosition = _grid.LocalToCell(transform.position);
        
        if (_previousObjectPosition == _currentObjectPosition) return;
        Debug.Log($"Vamos a casilla {_currentObjectPosition}");
        _eventMovement.occupier = gameObject;
        _eventMovement.NewPositionToMove = _currentObjectPosition;
        _eventMovement.OldPosition = _previousObjectPosition;
        _previousObjectPosition = _currentObjectPosition;
        _eventMovement.Fire();
        Debug.Log("FFFFFFFFFFFFF");
    }
}