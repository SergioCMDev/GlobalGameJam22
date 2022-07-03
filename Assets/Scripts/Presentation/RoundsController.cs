using System;
using App;
using App.Events;
using Presentation.Interfaces;
using Presentation.Managers;
using Presentation.UI.Menus;
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
        private ResourcesManager _resourcesManager;
        private PopupManager _popupManager;

        public event Action OnEnemyHasBeenDefeated;

        private void Start()
        {
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManager>();
            _popupManager = ServiceLocator.Instance.GetService<PopupManager>();
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
                //TODO CALCULATE QUANTITY TO ADD
                _resourcesManager.AddResources(RetrievableResourceType.Gold, 200);
                var newRoundPopup = _popupManager.InstantiatePopup<NewRoundPopup>(PopupType.NewRound);
                var closeablePopup = newRoundPopup.GetComponent<ICloseablePopup>();
                enemySpawner.HideEnemies();
                canvasPresenter.SetShowRangeButtonStatus(false);
                closeablePopup.PopupHasBeenClosed += StartNewRound;
                newRoundPopup.gameObject.SetActive(true);
                newRoundPopup.Init(timeToShowNewRoundPopup);
            }
            else
            {
                OnEnemyHasBeenDefeated?.Invoke();
            }
        }

        private bool NeedToPlayMoreRounds()
        {
            return _currentRound < _numberOfRoundsPerLevel;
        }
    }
}