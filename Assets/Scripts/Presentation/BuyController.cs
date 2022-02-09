using Application_;
using Application_.Events;
using Presentation.Managers;
using Presentation.Structs;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class BuyController : MonoBehaviour
    {
        private ResourcesTuple resourcesNeededForCurrentBuy;
        private BuildingType currentBuildingBuyType;
        private ResourcesManager _resourcesManager;
        private BuildingManager _buildingManager;

        [SerializeField] private PlayerSetBuildingInTilemapEvent _playerSetBuildingInTilemapEvent;
        private bool _playerIsCurrentlyBuying;
        void Start()
        {
            _buildingManager = ServiceLocator.Instance.GetService<BuildingManager>();
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManager>();
            _playerIsCurrentlyBuying = false;
        }

        public void PlayerWantsToBuyBuildingEvent(PlayerWantsToBuyBuildingEvent playerWantsToBuyBuildingEvent)
        {
            OnPlayerWantsToBuyBuilding(playerWantsToBuyBuildingEvent.BuildingType);
        }
        private void OnPlayerWantsToBuyBuilding(BuildingType buildingType)
        {
            if (_playerIsCurrentlyBuying) return;
            _playerIsCurrentlyBuying = true;
            //Get LastLevel of building
            var buildingsStatus = _buildingManager.GetBuildingStatus(buildingType);
            currentBuildingBuyType = buildingType;
            resourcesNeededForCurrentBuy =
                _buildingManager.GetResourcesForNextLevel(buildingsStatus.level, buildingsStatus.buildingType);
            if (!_resourcesManager.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold,
                    resourcesNeededForCurrentBuy.Metal)) return;

            OnFinishBuy(null);
        }



        private void OnFinishBuy(SelectedTileData obj)
        {
            Debug.Log($"Data Player has selected Tile");
            var prefab = _buildingManager.GetPrefabByBuildingType(currentBuildingBuyType);
            _playerSetBuildingInTilemapEvent.Prefab = prefab;
            // _playerSetBuildingInTilemapEvent.GridPosition = obj.GridPosition;
            _playerSetBuildingInTilemapEvent.Fire();
            // _buildingManager.UpgradeBoughtBuilding(currentBuildingBuyType);
            // _resourcesManager.RemoveResourcesOfPlayer(resourcesNeededForCurrentBuy);
            _playerIsCurrentlyBuying = false;

        }

        private void OnCancelBuy()
        {
            Debug.Log($"Canceled Buy");
            _playerIsCurrentlyBuying = false;
        }

        //TODO
    }
}