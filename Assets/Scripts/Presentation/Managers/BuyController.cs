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
        private MilitaryBuildingType _currentMilitaryBuildingBuyType;
        private ResourcesManager _resourcesManager;
        private MilitaryBuildingManager _militaryBuildingManager;

        [SerializeField] private AllowPlayerToSetBuildingInTilemapEvent _allowPlayerToSetBuildingInTilemapEvent;
        private bool _playerIsCurrentlyBuying;

        public bool PlayerIsCurrentlyBuying => _playerIsCurrentlyBuying;


        void Start()
        {
            _militaryBuildingManager = ServiceLocator.Instance.GetService<MilitaryBuildingManager>();
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManager>();
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
            if (!_resourcesManager.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold))
            {
                OnPlayerNeedMoreResources(resourcesNeededForCurrentBuy, militaryBuildingType);
                return;
            }
            var prefab = _militaryBuildingManager.GetPrefabByBuildingType(_currentMilitaryBuildingBuyType);
            OnPlayerCanBuyBuilding.Invoke(prefab, militaryBuildingType);
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
        
        public void AllowPlayerToBuy(GameObject prefab, MilitaryBuildingType militaryBuildingType)
        {
            _allowPlayerToSetBuildingInTilemapEvent.Prefab = prefab;
            _allowPlayerToSetBuildingInTilemapEvent.militaryBuildingType = militaryBuildingType;
            _allowPlayerToSetBuildingInTilemapEvent.Fire();
        }
    }
}