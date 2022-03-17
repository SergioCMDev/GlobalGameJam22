using System.Collections.Generic;
using App;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Managers
{
    public class GridMovementManager : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        // [SerializeField] private ObjectHasMovedToNewTileEvent _eventMovement;
        [SerializeField] private GridBuildingManager _gridBuildingManager;

        [SerializeField] private List<IMovable> _movables;

        [SerializeField] private TestMovement _testMovement;

        // Start is called before the first frame update
        void Start()
        {
            // foreach (var movable in _movables)
            // {
            //     movable.OnObjectMoved += OnObjectMoved;
            // }

            _testMovement.OnObjectMoved += OnObjectMoved;
        }

        private void OnObjectMoved(GameObject occupier, WorldPositionTuple worldPosition)
        {
            var gridPositions = WorldToGridPositionsConverter(worldPosition);
            // _eventMovement.Occupier = occupier;
            // _eventMovement.GridPositions = gridPositions;
            // // _eventMovement.OldPosition = gridPositions.OldGridPosition;
            // _eventMovement.Fire();
            _gridBuildingManager.ObjectHasMovedToNewTile(occupier, gridPositions);
        }

        private GridPositionTuple WorldToGridPositionsConverter(WorldPositionTuple worldPosition)
        {
            var gridPositionTuple = new GridPositionTuple()
            {
                NewGridPosition = _grid.LocalToCell(worldPosition.NewWorldPosition),
                OldGridPosition = _grid.LocalToCell(worldPosition.OldWorldPosition),
            };
            return gridPositionTuple;
        }
    }
}