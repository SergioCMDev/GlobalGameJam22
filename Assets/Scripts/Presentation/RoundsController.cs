using System;
using App;
using App.Events;
using Presentation.Managers;
using Presentation.UI;
using Presentation.UI.Menus;
using Services.Popups;
using Services.Popups.Interfaces;
using Services.ResourcesManager;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class RoundsController : MonoBehaviour
    {
        [SerializeField] private CanvasPresenter canvasPresenter;
        [SerializeField] private ActivateMilitaryBuildingsEvent activateMilitaryBuildingsEvent;
        [SerializeField] private ActivateMilitaryBuildingsEvent deactivateMilitaryBuildingsEvent;
        [SerializeField] private float _timeToDefendAgainstSlimes = 20, _timeToAllowPlayerBuildsTurrets;
        [SerializeField] private int _numberOfRoundsPerLevel;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private float timeToShowNewRoundPopup = 3;

        private int _currentRound;
        private ResourcesManagerService _resourcesManagerService;
        private PopupGenerator _popupManager;

        public event Action OnPlayerHasBeenDefeated;

        private void Start()
        {
            _resourcesManagerService = ServiceLocator.Instance.GetService<ResourcesManagerService>();
            _popupManager = ServiceLocator.Instance.GetService<PopupGenerator>();
        }

        public void StartNewRound()
        {
            _currentRound++;
            canvasPresenter.UpdateRoundInformation(_currentRound, _numberOfRoundsPerLevel);
            canvasPresenter.SetBuilderTimerInitialValue(_timeToAllowPlayerBuildsTurrets, ActivateEnemies);
            
            canvasPresenter.InitTimerLogic();
            canvasPresenter.SetBuildingSelectableViewStatus(true);
        }

        private void ActivateEnemies()
        {
            enemySpawner.ActivateEnemiesByTimer();
            activateMilitaryBuildingsEvent.Fire();
            canvasPresenter.CancelPendingActivitiesOfPlayer();
            canvasPresenter.SetDefensiveTimerInitialValue(_timeToDefendAgainstSlimes, RoundEnded);
            canvasPresenter.InitTimerLogic();
            canvasPresenter.SetBuildingSelectableViewStatus(false);
            canvasPresenter.SetShowRangeButtonStatus(true);
        }

        private void RoundEnded()
        {
            deactivateMilitaryBuildingsEvent.Fire();
            if (NeedToPlayMoreRounds())
            {
                // //TODO CALCULATE QUANTITY TO ADD
                _resourcesManagerService.AddResources(RetrievableResourceType.Gold, 200);
                var newRoundPopup = _popupManager.InstantiatePopup<NewRoundPopup>(PopupGenerator.PopupType.NewRound);
                var closeablePopup = newRoundPopup.GetComponent<ICloseablePopup>();
                enemySpawner.HideEnemies();
                canvasPresenter.SetShowRangeButtonStatus(false);
                closeablePopup.PopupHasBeenClosed += StartNewRound;
                newRoundPopup.gameObject.SetActive(true);
                newRoundPopup.Init(timeToShowNewRoundPopup);
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