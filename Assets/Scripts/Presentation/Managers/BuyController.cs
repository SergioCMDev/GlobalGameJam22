using System;
using App;
using App.Events;
using Presentation.Structs;
using Services.ResourcesManager;
using UnityEngine;
using Utils;

namespace Presentation.Managers
{
    public class BuyController : MonoBehaviour
    {
        private ResourcesTuple resourcesNeededForCurrentBuy;
        private MilitaryBuildingType _currentMilitaryBuildingBuyType;
        private ResourcesManagerService _resourcesManagerService;
        private MilitaryBuildingManager _militaryBuildingManager;

        [SerializeField] private AllowPlayerToSetBuildingInTilemapEvent _allowPlayerToSetBuildingInTilemapEvent;
        private bool _playerIsCurrentlyBuying;

        public bool PlayerIsCurrentlyBuying => _playerIsCurrentlyBuying;


        void Start()
        {
            _militaryBuildingManager = ServiceLocator.Instance.GetService<MilitaryBuildingManager>();
            _resourcesManagerService = ServiceLocator.Instance.GetService<ResourcesManagerService>();
            _playerIsCurrentlyBuying = false;
        }

        public void PlayerWantsToBuyBuilding(MilitaryBuildingType militaryBuildingType, Action<ResourcesTuple, MilitaryBuildingType> OnPlayerNeedMoreResources, 
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
            _allowPlayerToSetBuildingInTilemapEvent.Prefab = prefab;
            _allowPlayerToSetBuildingInTilemapEvent.militaryBuildingType = militaryBuildingType;
            _allowPlayerToSetBuildingInTilemapEvent.Fire();
        }
    }
}