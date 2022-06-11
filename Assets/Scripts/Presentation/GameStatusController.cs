using System.Collections.Generic;
using App.Events;
using App.SceneManagement;
using App.Services;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.Managers;
using Presentation.UI.Menus;
using UnityEngine;
using Utils;

namespace App.Managers
{

    public class RoundsLogic
    {
        
    }
    public class GameStatusController : MonoBehaviour
    {
        [SerializeField] private CanvasPresenter canvasPresenter;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private GridBuildingManager gridBuildingManager;
        [SerializeField] private List<BuildingPositionTuple> buildingPositionTuples;

        [SerializeField] private ShowWinMenuUIEvent showWinMenuUIEvent;
        [SerializeField] private ShowLostMenuUIEvent showLostMenuUIEvent;
        [SerializeField] private DeactivateMilitaryBuildingsEvent deactivateMilitaryBuildingsEvent;
        [SerializeField] private ActivateMilitaryBuildingsEvent activateMilitaryBuildingsEvent;
        [SerializeField] private float _timeToWin = 20, _timeToAllowPlayerBuildsTurrets;
        [SerializeField] private int _numberOfRoundsPerLevel;
        [SerializeField] private bool _skipTimer;
        private SceneChanger _sceneChanger;
        private SoundManager _soundManager;
        private GameDataService _gameDataService;
        private int _currentRound;
        private float _remainingTimeToWin;
        private bool _timerIsRunning;
        private List<Building> _buildings = new();

        private void Awake()
        {
            foreach (var VARIABLE in buildingPositionTuples)
            {
                _buildings.Add(VARIABLE.cityBuilding);
            }

            enemySpawner.SetCitiesToDestroy(_buildings);
        }

        void Start()
        {
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
            _soundManager = ServiceLocator.Instance.GetService<SoundManager>();
            _gameDataService = ServiceLocator.Instance.GetService<GameDataService>();

            enemySpawner.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;
            foreach (var cityBuilding in _buildings)
            {
                cityBuilding.OnBuildingDestroyed += CityHasBeenDestroyed;
            }

            gridBuildingManager.SetCitiesInGrid(buildingPositionTuples);

            StartNewRound();
        }

        private void StartNewRound()
        {
            _currentRound++;
            enemySpawner.HideEnemies();
            if (_skipTimer) return;
            canvasPresenter.SetBuilderTimerInitialValue(_timeToAllowPlayerBuildsTurrets, ActivateEnemies);
            canvasPresenter.InitTimerLogic(_skipTimer);
            canvasPresenter.SetBuildingSelectableViewStatus(true);
            //Poner enemigo en posicion inicial
        }

        private void ActivateEnemies()
        {
            enemySpawner.ActivateEnemiesByTimer();
            activateMilitaryBuildingsEvent.Fire();
            canvasPresenter.SetBuildingSelectableViewStatus(false);
            canvasPresenter.SetDefensiveTimerInitialValue(_timeToWin, RoundEnded);
        }

        private void RoundEnded()
        {
            deactivateMilitaryBuildingsEvent.Fire();
            if (NeedToPlayMoreRounds())
            {
                StartNewRound();
            }
            else
            {
                EnemyHasBeenDefeated();
            }
        }

        private bool NeedToPlayMoreRounds()
        {
            return _currentRound < _numberOfRoundsPerLevel;
        }


        //Refactor
        private void EnemyHasBeenDefeated(Enemy enemy = null)
        {
            enemySpawner.OnEnemyHasBeenDefeated -= EnemyHasBeenDefeated;

            _gameDataService.SaveGame(_sceneChanger.GetCurrentSceneName());
            _soundManager.PlaySfx(SfxSoundName.PlayerWinLevel);
            showWinMenuUIEvent.Fire();
            deactivateMilitaryBuildingsEvent.Fire();
            enemySpawner.StopEnemies();
        }

        private void CityHasBeenDestroyed(Building building)
        {
            _buildings.Remove(building);
            if (_buildings.Count != 0) return;
            LostLogic();
        }

        private void LostLogic()
        {
            _soundManager.PlaySfx(SfxSoundName.PlayerLoseLevel);
            showLostMenuUIEvent.Fire();
            deactivateMilitaryBuildingsEvent.Fire();
            enemySpawner.StopEnemies();
        }

        public void RestartLevel(PlayerHasRestartedLevelEvent levelEvent)
        {
            _sceneChanger.RestartScene(levelEvent);
        }

        public void WinLevel(PlayerHasWonLevelEvent levelEvent)
        {
            EnemyHasBeenDefeated();
        }

        public void LostLevel(PlayerHasLostLevelEvent levelEvent)
        {
            LostLogic();
        }
    }
}