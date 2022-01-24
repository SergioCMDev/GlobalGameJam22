using System;
using System.Collections.Generic;
using System.Linq;
using Application_;
using Application_.Models;
using Presentation.Managers;
using Presentation.Structs;
using Presentation.Utils;
using UnityEngine;

namespace Presentation
{
    public class BuyController : MonoBehaviour
    {
        [SerializeField] private BuildingsSelectable _buildingSelectable;
        [SerializeField] private ResourcesManager _resourcesManager;
        [SerializeField] private List<BuildingCost> _buildingCost;
        private IBuildingStatusModel _buildingStatusModel;
        private ResourcesTuple resourcesNeededForCurrentBuy;
        public event Action<BuildingCostTuple, Action, Action> OnStartSelectionOfNewPlaceForBuilding;

        // Start is called before the first frame update
        void Start()
        {
            _buildingSelectable.OnPlayerWantsToBuyBuilding += OnPlayerWantsToBuyBuilding;
        }

        private void OnPlayerWantsToBuyBuilding(BuildingPrefabTuple obj)
        {
            //Get LastLevel of building
            var buildingsStatus = GetBuildingStatus(obj);

            resourcesNeededForCurrentBuy = GetResourcesForNextLevel(buildingsStatus.level, buildingsStatus.buildingType);
            if (!_resourcesManager.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold, resourcesNeededForCurrentBuy.Metal)) return;
            OnStartSelectionOfNewPlaceForBuilding?.Invoke(new BuildingCostTuple
            {
                resourcesTuple = resourcesNeededForCurrentBuy,
                buildingPrefabTuple = obj,
            }, OnCancelBuy, OnFinishBuy);
            
            //Select where to place 
            //Update Model when building is set
        }

        private BuildStatus GetBuildingStatus(BuildingPrefabTuple obj)
        {
            return _buildingStatusModel.BuildStatusList.SingleOrDefault(x =>
                x.buildingType == obj.BuildingSelectable.BuildingType);
        }

        private void OnFinishBuy()
        {
            _resourcesManager.RemoveResourcesOfPlayer(resourcesNeededForCurrentBuy);
        }

        private void OnCancelBuy()
        {
            throw new NotImplementedException();
        }

        //TODO
        private ResourcesTuple GetResourcesForNextLevel(float buildStatusLevel, BuildingType buildStatusBuildingType)
        {
            var upgradeCost = _buildingCost.Single(X => X.BuildingType == buildStatusBuildingType);
            return new ResourcesTuple()
            {
                Gold = upgradeCost.GoldCostToUpgrade,
                Metal = upgradeCost.MetalCostToUpgrade
            };
        }
    }
}