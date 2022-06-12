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
        [SerializeField] private float _timeToWin = 20, _timeToAllowPlayerBuildsTurrets;
        [SerializeField] private int _numberOfRoundsPerLevel;
        [SerializeField] private EnemySpawner enemySpawner;

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
            canvasPresenter.SetDefensiveTimerInitialValue(_timeToWin, RoundEnded);
            canvasPresenter.InitTimerLogic();
            canvasPresenter.SetBuildingSelectableViewStatus(false);
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
                closeablePopup.PopupHasBeenClosed += StartNewRound;
                newRoundPopup.gameObject.SetActive(true);
                newRoundPopup.Init(5);
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