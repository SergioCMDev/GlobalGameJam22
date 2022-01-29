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
        [SerializeField] private List<BuildingCost> _buildingCost;

        [SerializeField] private List<BuildingDataTuple> buildingData;

        // Start is called before the first frame update
        void Start()
        {
            _buildingStatusModel = ServiceLocator.Instance.GetModel<IBuildingStatusModel>();
            _buildingStatusModel.AddBuilding(new BuildStatus()
            {
                buildingType = BuildingType.Tesla,
                MaxLife = 50,
            });
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
    }
}