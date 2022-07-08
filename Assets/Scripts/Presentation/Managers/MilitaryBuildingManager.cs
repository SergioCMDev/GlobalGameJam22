using System.Collections.Generic;
using System.Linq;
using App;
using App.Events;
using App.Models;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.Infrastructure.Scriptables;
using Presentation.Structs;
using UnityEngine;
using Utils;
namespace Presentation.Managers
{
    public class MilitaryBuildingManager : MonoBehaviour
    {
        [SerializeField] private List<BuildingCost> _buildingCost;
        [SerializeField] private List<BuildingDataTuple> buildingData;
        private IBuildingStatusModel _buildingStatusModel;
        private readonly List<MilitaryBuildingFacade> _ownMilitaryBuilding = new();

        void Start()
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
            var militaryBuilding = buildingEvent.Instance.GetComponent<MilitaryBuildingFacade>();
            militaryBuilding.Init();
            militaryBuilding.Deploy();
            _ownMilitaryBuilding.Add(militaryBuilding);
        }

        public void DeactivateMilitaryBuildings(DeactivateMilitaryBuildingsEvent deactivateMilitaryBuildingsEvent)
        {
            foreach (var building in _ownMilitaryBuilding)
            {
                building.Deactivate();
                building.CleanOccupiers();
            }
        }
        
        public void ActivateMilitaryBuildings(ActivateMilitaryBuildingsEvent stopMilitaryBuildingsEvent)
        {
            foreach (var building in _ownMilitaryBuilding)
            {
                building.ActivateBuilding();
            }
        }
    }
}