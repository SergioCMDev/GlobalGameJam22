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

        public void PlayerWantsToBuyBuilding(BuildingType buildingType, Action<ResourcesTuple, BuildingType> OnPlayerNeedMoreResources, 
            Action<GameObject, BuildingType> OnPlayerCanBuyBuilding)
        {
            if (_playerIsCurrentlyBuying) return;
            _playerIsCurrentlyBuying = true;
            var buildingsStatus = _militaryBuildingManager.GetBuildingStatus(buildingType);
            currentBuildingBuyType = buildingType;
            resourcesNeededForCurrentBuy =
                _militaryBuildingManager.GetResourcesForNextLevel(buildingsStatus.buildingType);
            if (!_resourcesManager.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold,
                    resourcesNeededForCurrentBuy.Metal))
            {
                OnPlayerNeedMoreResources(resourcesNeededForCurrentBuy, buildingType);
                return;
            }
            var prefab = _militaryBuildingManager.GetPrefabByBuildingType(currentBuildingBuyType);
            OnPlayerCanBuyBuilding.Invoke(prefab, buildingType);
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
        
        public void AllowPlayerToBuy(GameObject prefab, BuildingType buildingType)
        {
            _allowPlayerToSetBuildingInTilemapEvent.Prefab = prefab;
            _allowPlayerToSetBuildingInTilemapEvent.BuildingType = buildingType;
            _allowPlayerToSetBuildingInTilemapEvent.Fire();
        }
    }
}