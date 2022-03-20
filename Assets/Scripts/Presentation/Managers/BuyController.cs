using System;
using App;
using App.Events;
using Presentation.Structs;
using UnityEngine;
using Utils;

namespace Presentation.Managers
{
    public class BuyController : MonoBehaviour
    {
        private ResourcesTuple resourcesNeededForCurrentBuy;
        private BuildingType currentBuildingBuyType;
        private ResourcesManager _resourcesManager;
        private BuildingManager _buildingManager;

        [SerializeField] private AllowPlayerToSetBuildingInTilemapEvent _allowPlayerToSetBuildingInTilemapEvent;
        private bool _playerIsCurrentlyBuying;


        void Start()
        {
            _buildingManager = ServiceLocator.Instance.GetService<BuildingManager>();
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManager>();
            _playerIsCurrentlyBuying = false;
        }

        public void PlayerWantsToBuyBuilding(BuildingType buildingType, Action<ResourcesTuple, BuildingType> OnPlayerNeedMoreResources)
        {
            if (_playerIsCurrentlyBuying) return;
            _playerIsCurrentlyBuying = true;
            var buildingsStatus = _buildingManager.GetBuildingStatus(buildingType);
            currentBuildingBuyType = buildingType;
            resourcesNeededForCurrentBuy =
                _buildingManager.GetResourcesForNextLevel(buildingsStatus.level, buildingsStatus.buildingType);
            if (!_resourcesManager.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold,
                    resourcesNeededForCurrentBuy.Metal))
            {
                OnPlayerNeedMoreResources(resourcesNeededForCurrentBuy, buildingType);
                return;
            }

            var prefab = _buildingManager.GetPrefabByBuildingType(currentBuildingBuyType);
            _allowPlayerToSetBuildingInTilemapEvent.Prefab = prefab;
            _allowPlayerToSetBuildingInTilemapEvent.BuildingType = buildingType;
            _allowPlayerToSetBuildingInTilemapEvent.Fire();
        }

        public void EndBuyCorrectly()
        {
            _resourcesManager.RemoveResourcesOfPlayer(resourcesNeededForCurrentBuy);
            _playerIsCurrentlyBuying = false;
        }

        public void BuyHasBeenCanceled()
        {
            _playerIsCurrentlyBuying = false;
        }
    }
}