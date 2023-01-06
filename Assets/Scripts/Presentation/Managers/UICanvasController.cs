using App.Buildings;
using App.Events;
using App.Resources;
using Presentation.Infrastructure;
using Presentation.UI.Menus;
using UnityEngine;

namespace Presentation.Managers
{
    public class UICanvasController : MonoBehaviour
    {
        [SerializeField] private CanvasPresenter _canvasPresenter;
        [SerializeField] private GameStatusController gameStatusController;
        [SerializeField] private AllowPlayerToSetBuildingInTilemapEvent allowPlayerToSetBuildingInTilemapEvent;

        private BuyController _buyController;
        private GridBuildingManager _gridBuildingManager;

        // private SoundPlayer _soundPlayer;

        void Start()
        {
            _buyController = new BuyController();
            _buyController.Init();
            _buyController.OnAllowPlayerToSetBuildingInTilemap = AllowPlayerToSetBuildingInTilemap;
            _gridBuildingManager = gameStatusController.GridBuildingManager;
            // _soundPlayer = ServiceLocator.Instance.GetService<SoundPlayer>();
            _canvasPresenter.OnPlayerWantsToSetBuildingInGrid += PlayerWantsToSetBuildingInGrid;
            _canvasPresenter.OnSystemCancelsBuy += CancelBuildingSetting;
            _gridBuildingManager.OnPlayerHasSetBuildingOnGrid += PlayerHasSetBuildingInGrid;
            _gridBuildingManager.OnPlayerHasCanceledSetBuildingOnGrid += PlayerHasCanceledSetBuildingInGrid;
        }

        private void AllowPlayerToSetBuildingInTilemap(AllowPlayerToSetBuildingInTilemapData obj)
        {
            allowPlayerToSetBuildingInTilemapEvent.militaryBuildingType = obj.MilitaryBuildingType;
            allowPlayerToSetBuildingInTilemapEvent.Prefab = obj.Prefab;
            allowPlayerToSetBuildingInTilemapEvent.Fire();
        }

        private void CancelBuildingSetting()
        {
            if (!_buyController.PlayerIsCurrentlyBuying) return;

            _gridBuildingManager.CancelTakingPlace();
            _buyController.BuyHasBeenCanceled();
        }

        private void PlayerHasCanceledSetBuildingInGrid()
        {
            // _soundPlayer.PlaySfx(SfxSoundName.BuyCanceled);

            _canvasPresenter.SetBuildingSelectableViewStatus(true);
            _buyController.BuyHasBeenCanceled();
        }

        private void PlayerHasSetBuildingInGrid(MilitaryBuildingFacade militaryBuildingFacade)
        {
            // _soundPlayer.PlaySfx(SfxSoundName.BuildingHasBeenSet);

            _canvasPresenter.SetBuildingSelectableViewStatus(true);
            _buyController.EndBuyCorrectly();
        }

        private void PlayerWantsToSetBuildingInGrid(MilitaryBuildingType militaryBuildingType)
        {
            _buyController.PlayerWantsToBuyBuilding(militaryBuildingType, OnPlayerNeedsMoreResources,
                OnPlayerCanBuyBuilding);
        }

        private void OnPlayerCanBuyBuilding(GameObject prefab, MilitaryBuildingType militaryBuildingType)
        {
            _canvasPresenter.SetBuildingSelectableViewStatus(false);

            _buyController.AllowPlayerToBuy(prefab, militaryBuildingType);
        }

        private void OnPlayerNeedsMoreResources(ResourcesTuple resourcesNeeded,
            MilitaryBuildingType militaryBuildingType)
        {
            _canvasPresenter.ShowNeedMoreResourcesPanel(resourcesNeeded, militaryBuildingType);

            // _soundPlayer.PlaySfx(SfxSoundName.PlayerNeedsMoreResources);

            _canvasPresenter.SetBuildingSelectableViewStatus(true);
            _buyController.BuyHasBeenCanceled();
        }
    }
}