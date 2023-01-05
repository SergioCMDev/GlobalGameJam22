using System.Collections.Generic;
using App;
using App.Buildings;
using App.Info.Tuples;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.Interfaces;
using Presentation.Structs;
using UnityEngine;

namespace Presentation.Managers
{
    public class GridMovementManager
    {
        private Grid _grid;

        private GridBuildingManager _gridBuildingManager;
        private TestMovement _testMovement;

        public void Init(GridMovementManagerInitData gridMovementManagerInitData)
        {
            _grid = gridMovementManagerInitData.grid;
            _gridBuildingManager = gridMovementManagerInitData.gridBuildingManager;
            _testMovement = gridMovementManagerInitData.testMovement;

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
            _gridBuildingManager.GetAnyTurretWhichHasTileInRange(tile, out var buildings);
            foreach (var building in buildings)
            {
                if (building.Type != MilitaryBuildingType.Tesla) continue;
                var teslaBuilding = building.GetComponent<TeslaMilitaryBuildingFacade>();
                teslaBuilding.CheckIfEnemyIsOutside(occupier);
            }
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

    public struct GridMovementManagerInitData
    {
        public Grid grid;

        public GridBuildingManager gridBuildingManager;
        public TestMovement testMovement;
    }
}