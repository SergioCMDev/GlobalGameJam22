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
        [SerializeField] private BuildingsSelectable _buildingSelectable;
        [SerializeField] private ResourcesManager _resourcesManager;
        [SerializeField] private InputTileManager inputTileManager;
        
        private ResourcesTuple resourcesNeededForCurrentBuy;
        private BuildingType currentBuildingBuyType;
        private BuildingManager _buildingManager;

        [SerializeField] private PlayerSetBuildingInTilemapEvent _playerSetBuildingInTilemapEvent;

        void Start()
        {
            _buildingSelectable.OnPlayerWantsToBuyBuilding += OnPlayerWantsToBuyBuilding;
            _buildingManager = ServiceLocator.Instance.GetService<BuildingManager>();
        }

        private void OnPlayerWantsToBuyBuilding(BuildingTypeTuple buildingTypeTuple)
        {
            //Get LastLevel of building
            var buildingsStatus = _buildingManager.GetBuildingStatus(buildingTypeTuple);
            currentBuildingBuyType = buildingTypeTuple.BuildingSelectable.BuildingType;
            resourcesNeededForCurrentBuy =
                _buildingManager.GetResourcesForNextLevel(buildingsStatus.level, buildingsStatus.buildingType);
            if (!_resourcesManager.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold,
                    resourcesNeededForCurrentBuy.Metal)) return;
            inputTileManager.EnableTileSelection(OnCancelBuy, OnFinishBuy, OnTileOccupied);
        }

        private void OnTileOccupied()
        {
            Debug.Log($"SELECTED TILE Is Occupied");
            inputTileManager.EnableTileSelection(OnCancelBuy, OnFinishBuy, OnTileOccupied);
        }

        private void OnFinishBuy(SelectedTileData obj)
        {
            Debug.Log($"Data Player has selected Tile");
            var prefab = _buildingManager.GetPrefabByBuildingType(currentBuildingBuyType);
            _playerSetBuildingInTilemapEvent.Prefab = prefab;
            _playerSetBuildingInTilemapEvent.SelectedTile = obj;
            _playerSetBuildingInTilemapEvent.Fire();
            _buildingManager.UpgradeBoughtBuilding(currentBuildingBuyType);
            _resourcesManager.RemoveResourcesOfPlayer(resourcesNeededForCurrentBuy);
        }


        private void OnCancelBuy()
        {
            Debug.Log($"Canceled Buy");
        }

        //TODO
    }
}