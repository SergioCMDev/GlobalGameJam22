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
        private MiltaryBuildingType _currentMiltaryBuildingBuyType;
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

        public void PlayerWantsToBuyBuilding(MiltaryBuildingType militaryBuildingType, Action<ResourcesTuple, MiltaryBuildingType> OnPlayerNeedMoreResources, 
            Action<GameObject, MiltaryBuildingType> OnPlayerCanBuyBuilding)
        {
            if (_playerIsCurrentlyBuying) return;
            _playerIsCurrentlyBuying = true;
            var buildingsStatus = _militaryBuildingManager.GetBuildingStatus(militaryBuildingType);
            _currentMiltaryBuildingBuyType = militaryBuildingType;
            resourcesNeededForCurrentBuy =
                _militaryBuildingManager.GetResourcesForNextLevel(buildingsStatus.MilitaryBuildingType);
            if (!_resourcesManager.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold,
                    resourcesNeededForCurrentBuy.Metal))
            {
                OnPlayerNeedMoreResources(resourcesNeededForCurrentBuy, militaryBuildingType);
                return;
            }
            var prefab = _militaryBuildingManager.GetPrefabByBuildingType(_currentMiltaryBuildingBuyType);
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
        
        public void AllowPlayerToBuy(GameObject prefab, MiltaryBuildingType miltaryBuildingType)
        {
            _allowPlayerToSetBuildingInTilemapEvent.Prefab = prefab;
            _allowPlayerToSetBuildingInTilemapEvent.miltaryBuildingType = miltaryBuildingType;
            _allowPlayerToSetBuildingInTilemapEvent.Fire();
        }
    }
}