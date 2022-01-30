using System.Collections.Generic;
using System.Linq;
using Application_;
using Application_.Models;
using Presentation.Structs;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class BuildingManager : MonoBehaviour
    {
        private IBuildingStatusModel _buildingStatusModel;
        private GameObject _enemy;
        private IReceiveDamage _enemyReceiveDamage;
        private List<MilitaryBuilding> _ownMilitaryBuilding = new List<MilitaryBuilding>();
        [SerializeField] private List<BuildingCost> _buildingCost;

        [SerializeField] private List<BuildingDataTuple> buildingData;

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

            _enemy = FindObjectOfType<Enemy>().gameObject;
            _enemyReceiveDamage = _enemy.GetComponent<IReceiveDamage>();
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

        public BuildStatus GetBuildingStatus(BuildingTypeTuple obj)
        {
            return _buildingStatusModel.BuildStatusList.SingleOrDefault(x =>
                x.buildingType == obj.BuildingSelectable.BuildingType);
        }

        public void SaveBuilding(SaveBuildingEvent tilemapEvent)
        {
            var militaryBuilding = tilemapEvent.Instance.GetComponent<MilitaryBuilding>();
            militaryBuilding.SetEnemyToAttack(_enemyReceiveDamage);
            _ownMilitaryBuilding.Add(militaryBuilding);
        }
    }
}