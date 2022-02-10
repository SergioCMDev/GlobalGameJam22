using System.Collections.Generic;
using System.Linq;
using Application_;
using Application_.Events;
using Application_.Models;
using Presentation.Building;
using Presentation.Structs;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private List<BuildingCost> _buildingCost;
        [SerializeField] private List<BuildingDataTuple> buildingData;
        private IBuildingStatusModel _buildingStatusModel;
        private List<MilitaryBuilding> _ownMilitaryBuilding = new List<MilitaryBuilding>();

        private GameObject _enemy;

        void Start()
        {
            _buildingStatusModel = ServiceLocator.Instance.GetModel<IBuildingStatusModel>();
            foreach (var building in buildingData)
            {
                _buildingStatusModel.AddBuilding(new BuildStatus()
                {
                    buildingType = building.BuildingType,
                    MaxLife = 50,
                });
            }

            if(FindObjectOfType<Enemy.Enemy>())
                _enemy = FindObjectOfType<Enemy.Enemy>().gameObject;
        }

        public GameObject GetPrefabByBuildingType(BuildingType type)
        {
            return buildingData.Single(x => x.BuildingType == type).Prefab;
        }

        public void UpgradeBoughtBuilding(BuildingType type)
        {
            _buildingStatusModel.BuildStatusList.SingleOrDefault(x =>
                    x.buildingType == type)
                ?.Upgrade();
        }

        public ResourcesTuple GetResourcesForNextLevel(float buildStatusLevel, BuildingType buildStatusBuildingType)
        {
            var upgradeCost = _buildingCost.Single(X => X.BuildingType == buildStatusBuildingType);
            return new ResourcesTuple()
            {
                Gold = upgradeCost.GoldCostToUpgrade,
                Metal = upgradeCost.MetalCostToUpgrade
            };
        }

        public BuildStatus GetBuildingStatus(BuildingType type)
        {
            return _buildingStatusModel.BuildStatusList.SingleOrDefault(x =>
                x.buildingType == type);
        }

        public void SaveBuilding(SaveBuildingEvent tilemapEvent)
        {
            var militaryBuilding = tilemapEvent.Instance.GetComponent<MilitaryBuilding>();
            militaryBuilding.SetEnemyToAttack(_enemy);
            _ownMilitaryBuilding.Add(militaryBuilding);
        }
    }
}