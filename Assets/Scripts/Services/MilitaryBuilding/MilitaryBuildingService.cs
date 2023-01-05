using System.Collections.Generic;
using System.Linq;
using App;
using App.Buildings;
using App.Events;
using App.Models;
using App.Resources;
using UnityEngine;
using Utils;

namespace Services.MilitaryBuilding
{
    [CreateAssetMenu(fileName = "MilitaryBuildingService",
        menuName = "Loadable/Services/MilitaryBuildingService")]
    public class MilitaryBuildingService : LoadableComponent
    {
        [SerializeField] private List<BuildingCost> _buildingCost;
        [SerializeField] private List<BuildingDataTuple> buildingData;
        private IBuildingStatusModel _buildingStatusModel;
        private readonly List<IMilitaryBuilding> _ownMilitaryBuilding = new();

        public GameObject GetPrefabByBuildingType(MilitaryBuildingType type)
        {
            return buildingData.Single(x => x.militaryBuildingType == type).Prefab;
        }

        public ResourcesTuple GetResourcesForNextLevel(MilitaryBuildingType buildStatusMilitaryBuildingType)
        {
            var upgradeCost = _buildingCost.Single(x => x.militaryBuildingType == buildStatusMilitaryBuildingType);
            return new ResourcesTuple()
            {
                Gold = upgradeCost.GoldCostToUpgrade,
                Metal = upgradeCost.MetalCostToUpgrade
            };
        }

        public BuildStatus GetBuildingStatus(MilitaryBuildingType type)
        {
            return _buildingStatusModel.BuildStatusList.SingleOrDefault(x =>
                x.MilitaryBuildingType == type);
        }

        public void SaveBuilding(SaveBuildingEvent buildingEvent)
        {
            var militaryBuilding = buildingEvent.Instance.GetComponent<IMilitaryBuilding>();
            militaryBuilding.Init();
            militaryBuilding.Deploy();
            _ownMilitaryBuilding.Add(militaryBuilding);
        }

        public void DeactivateMilitaryBuildings()
        {
            foreach (var building in _ownMilitaryBuilding)
            {
                building.Deactivate();
                building.CleanOccupiers();
            }
        }

        public void ActivateMilitaryBuildings()
        {
            foreach (var building in _ownMilitaryBuilding)
            {
                building.ActivateBuilding();
            }
        }

        public override void Execute()
        {
            _buildingStatusModel = ServiceLocator.Instance.GetModel<IBuildingStatusModel>();
            foreach (var building in buildingData)
            {
                _buildingStatusModel.AddBuilding(new BuildStatus()
                {
                    MilitaryBuildingType = building.militaryBuildingType,
                    MaxLife = 50,
                });
            }
        }
    }
}