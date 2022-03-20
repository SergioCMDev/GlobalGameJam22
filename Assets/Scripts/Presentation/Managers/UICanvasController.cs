using App;
using App.Events;
using Presentation.Infrastructure;
using Presentation.Structs;
using Presentation.UI.Menus;
using UnityEngine;
using Utils;

namespace Presentation.Managers
{
    public class UICanvasController : MonoBehaviour
    {
        [SerializeField] private CanvasPresenter _canvasPresenter;

        [SerializeField] private BuyController _buyController;
        [SerializeField] private GridBuildingManager _gridBuildingManager;

        private SoundManager _soundManager;

        void Start()
        {
            _soundManager = ServiceLocator.Instance.GetService<SoundManager>();
            _canvasPresenter.OnPlayerWantsToSetBuildingInGrid += PlayerWantsToSetBuildingInGrid;
            _gridBuildingManager.OnPlayerHasSetBuildingOnGrid += PlayerHasSetBuildingInGrid;
            _gridBuildingManager.OnPlayerHasCanceledSetBuildingOnGrid += PlayerHasCanceledSetBuildingInGrid;
        }

        private void PlayerHasCanceledSetBuildingInGrid()
        {
            _soundManager.PlaySfx(SfxSoundName.BuyCanceled);

            _canvasPresenter.SetBuildingSelectableStatus(true);
            _buyController.BuyHasBeenCanceled();
        }

        private void PlayerHasSetBuildingInGrid(MilitaryBuildingFacade militaryBuildingFacade)
        {
            _soundManager.PlaySfx(SfxSoundName.BuildingHasBeenSet);

            _canvasPresenter.SetBuildingSelectableStatus(true);
            _buyController.EndBuyCorrectly();
        }

        private void PlayerWantsToSetBuildingInGrid(BuildingType buildingType)
        {
            _buyController.PlayerWantsToBuyBuilding(buildingType, OnPlayerNeedsMoreResources);
        }

        private void OnPlayerNeedsMoreResources(ResourcesTuple resourcesNeeded, BuildingType buildingType)
        {
            _canvasPresenter.ShowNeedMoreResourcesPanel(resourcesNeeded, buildingType);

            // _soundManager.PlaySfx(SfxSoundName.PlayerNeedsMoreResources);
        
            _canvasPresenter.SetBuildingSelectableStatus(true);
            _buyController.BuyHasBeenCanceled();
        }
    
    }
}