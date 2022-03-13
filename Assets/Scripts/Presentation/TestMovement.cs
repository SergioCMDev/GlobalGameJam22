using System;
using App.Events;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField] private ObjectHasMovedToNewTileEvent _eventMovement;
    [SerializeField] private Grid _grid;
    // private Vector3Int _objectGridPosition;
    private Vector3 _previousObjectWorldPosition;
    public event Action OnObjectMoved;


    private void Start()
    {
        _previousObjectWorldPosition = transform.position;
    }

    void Update()
    {
        if (_previousObjectWorldPosition == transform.position) return;
        var objectWorldPosition = transform.position;
        var objectGridPosition = _grid.LocalToCell(objectWorldPosition);
        OnObjectMoved?.Invoke();
        Debug.Log($"Vamos a casilla {objectGridPosition}");
        _eventMovement.Occupier = gameObject;
        _eventMovement.NewPositionToMove = objectGridPosition;
        _eventMovement.OldPosition = _grid.LocalToCell(_previousObjectWorldPosition);
        _previousObjectWorldPosition = objectWorldPosition;
        _eventMovement.Fire();
    }
}