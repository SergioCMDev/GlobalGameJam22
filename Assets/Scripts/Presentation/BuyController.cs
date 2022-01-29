using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Application_;
using Application_.Events;
using Application_.Models;
using Presentation.Managers;
using Presentation.Structs;
using Presentation.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Presentation
{
    public class BuyController : MonoBehaviour
    {
        [SerializeField] private BuildingsSelectable _buildingSelectable;
        [SerializeField] private ResourcesManager _resourcesManager;
        [SerializeField] private InputTileManager inputTileManager;
        [SerializeField] private List<BuildingCost> _buildingCost;
        private IBuildingStatusModel _buildingStatusModel;
        private ResourcesTuple resourcesNeededForCurrentBuy;
        private BuildingType currentBuildingBuyType;
        [SerializeField] private GameObject prefab;
        [SerializeField] private PlayerSetBuildingInTilemapEvent _playerSetBuildingInTilemapEvent;
        public event Action<BuildingCostTuple, Action, Action> OnStartSelectionOfNewPlaceForBuilding;

        // Start is called before the first frame update
        void Start()
        {
            _buildingSelectable.OnPlayerWantsToBuyBuilding += OnPlayerWantsToBuyBuilding;
            _buildingStatusModel = ServiceLocator.Instance.GetModel<IBuildingStatusModel>();
        }

        private void OnPlayerWantsToBuyBuilding(BuildingPrefabTuple buildingPrefabTuple)
        {
            //Get LastLevel of building
            var buildingsStatus = GetBuildingStatus(buildingPrefabTuple);
            currentBuildingBuyType = buildingPrefabTuple.BuildingSelectable.BuildingType;
            prefab = buildingPrefabTuple.BuildingPrefab;
            resourcesNeededForCurrentBuy =
                GetResourcesForNextLevel(buildingsStatus.level, buildingsStatus.buildingType);
            if (!_resourcesManager.PlayerHasEnoughResources(resourcesNeededForCurrentBuy.Gold,
                    resourcesNeededForCurrentBuy.Metal)) return;
            inputTileManager.EnableTileSelection(OnCancelBuy, OnFinishBuy, OnTileOccupied);
            // OnStartSelectionOfNewPlaceForBuilding?.Invoke(new BuildingCostTuple
            // {
            //     resourcesTuple = resourcesNeededForCurrentBuy,
            //     buildingPrefabTuple = buildingPrefabTuple,
            // }, OnCancelBuy, OnFinishBuy);

            //Select where to place 
            //Update Model when building is set
        }

        private void OnTileOccupied()
        {
            Debug.Log($"SELECTED TILE Is Occupied");
            inputTileManager.EnableTileSelection(OnCancelBuy, OnFinishBuy, OnTileOccupied);
        }

        private void OnFinishBuy(SelectedTileData obj)
        {
            Debug.Log($"Data Player has selected Tile");
            _playerSetBuildingInTilemapEvent.Prefab = prefab;
            _playerSetBuildingInTilemapEvent.SelectedTile = obj;
            _playerSetBuildingInTilemapEvent.Fire();
            UpgradeBoughtBuilding();
            _resourcesManager.RemoveResourcesOfPlayer(resourcesNeededForCurrentBuy);
        }


        private BuildStatus GetBuildingStatus(BuildingPrefabTuple obj)
        {
            return _buildingStatusModel.BuildStatusList.SingleOrDefault(x =>
                x.buildingType == obj.BuildingSelectable.BuildingType);
        }


        private void UpgradeBoughtBuilding()
        {
            _buildingStatusModel.BuildStatusList.SingleOrDefault(x =>
                    x.buildingType == currentBuildingBuyType)
                ?.Upgrade();
        }

        private void OnCancelBuy()
        {
            Debug.Log($"Canceled Buy");
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