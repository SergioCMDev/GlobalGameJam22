using System;
using App.Resources;
using Presentation.Managers;
using Presentation.UI;
using Presentation.UI.Menus;
using Services.Popups;
using Services.Popups.Interfaces;
using Services.ResourcesManager;
using Utils;

namespace Presentation
{
    public class RoundsController
    {
        public struct RoundsControllerInitData
        {
            public CanvasPresenter CanvasPresenter;
            public float TimeToDefendAgainstSlimes;
            public float TimeToAllowPlayerBuildsTurrets;
            public float TimeToShowNewRoundPopup;
            public int NumberOfRoundsPerLevel;
            public EnemySpawner EnemySpawner;
            public SlidersLogic SlidersLogic;
        }

        private CanvasPresenter _canvasPresenter;
        private SlidersLogic slidersLogic;
        private float _timeToDefendAgainstSlimes = 20, _timeToAllowPlayerBuildsTurrets, _timeToShowNewRoundPopup = 3;
        private int _numberOfRoundsPerLevel;
        private EnemySpawner _enemySpawner;


        private int _currentRound;
        private ResourcesManagerService _resourcesManagerService;
        private PopupGenerator _popupManager;

        public Action OnPlayerHasBeenDefeated, OnActivateMilitaryBuildings, OnDeactivateMilitaryBuildings;

        public void Init(RoundsControllerInitData roundsControllerInitData)
        {
            _resourcesManagerService = ServiceLocator.Instance.GetService<ResourcesManagerService>();
            _popupManager = ServiceLocator.Instance.GetService<PopupGenerator>();

            _canvasPresenter = roundsControllerInitData.CanvasPresenter;
            slidersLogic = roundsControllerInitData.SlidersLogic;
            _timeToDefendAgainstSlimes = roundsControllerInitData.TimeToDefendAgainstSlimes;
            _timeToAllowPlayerBuildsTurrets = roundsControllerInitData.TimeToAllowPlayerBuildsTurrets;
            _timeToShowNewRoundPopup = roundsControllerInitData.TimeToShowNewRoundPopup;
            _numberOfRoundsPerLevel = roundsControllerInitData.NumberOfRoundsPerLevel;
            _enemySpawner = roundsControllerInitData.EnemySpawner;
        }

        public void StartNewRound()
        {
            _currentRound++;
            _canvasPresenter.UpdateRoundInformation(_currentRound, _numberOfRoundsPerLevel);
            slidersLogic.SetBuilderTimerInitialValue(_timeToAllowPlayerBuildsTurrets, ActivateEnemies);

            slidersLogic.InitTimerLogic();
            _canvasPresenter.SetBuildingSelectableViewStatus(true);
        }

        private void ActivateEnemies()
        {
            _enemySpawner.ActivateEnemiesByTimer();

            OnActivateMilitaryBuildings?.Invoke();
            _canvasPresenter.CancelPendingActivitiesOfPlayer();
            slidersLogic.SetDefensiveTimerInitialValue(_timeToDefendAgainstSlimes, RoundEnded);
            slidersLogic.InitTimerLogic();
            _canvasPresenter.SetBuildingSelectableViewStatus(false);
            _canvasPresenter.SetShowRangeButtonStatus(true);
        }

        private void RoundEnded()
        {
            OnDeactivateMilitaryBuildings?.Invoke();
            if (NeedToPlayMoreRounds())
            {
                // //TODO CALCULATE QUANTITY TO ADD
                _resourcesManagerService.AddResources(RetrievableResourceType.Gold, 200);
                var newRoundPopup = _popupManager.InstantiatePopup<NewRoundPopup>(PopupGenerator.PopupType.NewRound);
                var closeablePopup = newRoundPopup.GetComponent<ICloseablePopup>();
                _enemySpawner.HideEnemies();
                _canvasPresenter.SetShowRangeButtonStatus(false);
                closeablePopup.PopupHasBeenClosed += StartNewRound;
                newRoundPopup.gameObject.SetActive(true);
                newRoundPopup.Init(_timeToShowNewRoundPopup);
            }
            else
            {
                OnPlayerHasBeenDefeated?.Invoke();
            }
        }

        private bool NeedToPlayMoreRounds()
        {
            return _currentRound < _numberOfRoundsPerLevel;
        }
    }
}