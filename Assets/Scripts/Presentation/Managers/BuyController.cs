using System;
using App.Buildings;
using App.Resources;
using Services.MilitaryBuilding;
using Services.ResourcesManager;
using UnityEngine;
using Utils;

namespace Presentation.Managers
{
    public class BuyController
    {
        private ResourcesTuple resourcesNeededForCurrentBuy;
        private MilitaryBuildingType _currentMilitaryBuildingBuyType;
        private ResourcesManagerService _resourcesManagerService;
        private MilitaryBuildingService _militaryBuildingManager;

        private bool _playerIsCurrentlyBuying;

        public bool PlayerIsCurrentlyBuying => _playerIsCurrentlyBuying;

        public Action<AllowPlayerToSetBuildingInTilemapData> OnAllowPlayerToSetBuildingInTilemap;

        public void Init()
        {
            _militaryBuildingManager = ServiceLocator.Instance.GetService<MilitaryBuildingService>();
            _resourcesManagerService = ServiceLocator.Instance.GetService<ResourcesManagerService>();
            _playerIsCurrentlyBuying = false;
        }

        public void PlayerWantsToBuyBuilding(MilitaryBuildingType militaryBuildingType,
            Action<ResourcesTuple, MilitaryBuildingType> OnPlayerNeedMoreResources,
            Action<GameObject, MilitaryBuildingType> OnPlayerCanBuyBuilding)
        {
            if (_playerIsCurrentlyBuying) return;
            _playerIsCurrentlyBuying = true;
            var buildingsStatus = _militaryBuildingManager.GetBuildingStatus(militaryBuildingType);
            _currentMilitaryBuildingBuyType = militaryBuildingType;
            resourcesNeededForCurrentBuy =
                _militaryBuildingManager.GetResourcesForNextLevel(buildingsStatus.MilitaryBuildingType);
            if (!_resourcesManagerService.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold))
            {
                OnPlayerNeedMoreResources(resourcesNeededForCurrentBuy, militaryBuildingType);
                return;
            }

            var prefab = _militaryBuildingManager.GetPrefabByBuildingType(_currentMilitaryBuildingBuyType);
            OnPlayerCanBuyBuilding.Invoke(prefab, militaryBuildingType);
        }

        public void EndBuyCorrectly()
        {
            _resourcesManagerService.RemoveResourcesOfPlayer(resourcesNeededForCurrentBuy);
            _playerIsCurrentlyBuying = false;
        }

        public void BuyHasBeenCanceled()
        {
            _playerIsCurrentlyBuying = false;
        }

        public void AllowPlayerToBuy(GameObject prefab, MilitaryBuildingType militaryBuildingType)
        {
            OnAllowPlayerToSetBuildingInTilemap?.Invoke(new AllowPlayerToSetBuildingInTilemapData
            {
                Prefab = prefab,
                MilitaryBuildingType = militaryBuildingType,
            });
        }
    }
}

public struct AllowPlayerToSetBuildingInTilemapData
{
    public GameObject Prefab;
    public MilitaryBuildingType MilitaryBuildingType;
}