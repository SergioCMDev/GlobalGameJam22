using System.Collections.Generic;
using App;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.Interfaces;
using Presentation.Structs;
using UnityEngine;

namespace Presentation.Managers
{
    public class GridMovementManager : MonoBehaviour
    {
        [SerializeField] private Grid _grid;

        // [SerializeField] private ObjectHasMovedToNewTileEvent _eventMovement;
        [SerializeField] private GridBuildingManager _gridBuildingManager;
        [SerializeField] private TestMovement _testMovement;

        // Start is called before the first frame update
        void Start()
        {
            if (!_testMovement) return;
            _testMovement.OnObjectMoved += OnObjectMoved;
        }

        public void OnObjectMoved(GameObject occupier, WorldPositionTuple worldPosition)
        {
            var gridPositions = WorldToGridPositionsConverter(worldPosition);
            //Check if was in the range of any turret

            _gridBuildingManager.ObjectHasMovedToNewTile(occupier, gridPositions);

            if (!_gridBuildingManager.WorldTileDictionary.ContainsKey(gridPositions.OldGridPosition)) return;
            var tile = _gridBuildingManager.WorldTileDictionary[gridPositions.OldGridPosition];
            _gridBuildingManager.GetAnyTurretWhichHasTileInRange(tile, out var building);
            if (building == null || building.Type != MilitaryBuildingType.Tesla) return;
            var teslaBuilding = building.GetComponent<TeslaMilitaryBuildingFacade>();
            teslaBuilding.CheckIfEnemyIsOutside(occupier);
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