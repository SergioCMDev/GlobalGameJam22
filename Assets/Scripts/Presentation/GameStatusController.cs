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
    public class GameStatusController : MonoBehaviour
    {
        [SerializeField] private CanvasPresenter _canvasPresenter;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private GridBuildingManager gridBuildingManager;
        [SerializeField] private List<BuildingPositionTuple> buildingPositionTuples;

        [SerializeField] private ShowWinMenuUIEvent showWinMenuUIEvent;
        [SerializeField] private ShowLostMenuUIEvent showLostMenuUIEvent;
        [SerializeField] private StopMilitaryBuildingsEvent stopMilitaryBuildingsEvent;
        [SerializeField] private float _timeToWin = 20, _timeToAllowPlayerBuildsTurrets;
        [SerializeField] private bool _skipTimer;
        private SceneChanger _sceneChanger;
        private SoundManager _soundManager;
        private GameDataService _gameDataService;
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
            if(_skipTimer) return;
            _canvasPresenter.SetBuilderTimerInitialValue(_timeToAllowPlayerBuildsTurrets);
            _canvasPresenter.InitTimerLogic(_skipTimer);
            _canvasPresenter.OnTimerHasEnd += DefensiveTimerHasEnded;
            gridBuildingManager.SetCitiesInGrid(buildingPositionTuples);
        }

        private void DefensiveTimerHasEnded()
        {
            _canvasPresenter.OnTimerHasEnd -= DefensiveTimerHasEnded;
            enemySpawner.ActivateEnemies();
            _canvasPresenter.DisableTurretsView();
            _canvasPresenter.SetDefensiveTimerInitialValue(_timeToWin);
            _canvasPresenter.OnTimerHasEnd += EnemyHasBeenDefeatedByTimer;
        }

        private void EnemyHasBeenDefeatedByTimer()
        {
            _canvasPresenter.OnTimerHasEnd -= EnemyHasBeenDefeatedByTimer;
            EnemyHasBeenDefeated(null);
        }
        

//Refactor
        private void EnemyHasBeenDefeated(Enemy enemy)
        {
            _gameDataService.SaveGame(_sceneChanger.GetCurrentSceneName());
            _soundManager.PlaySfx(SfxSoundName.PlayerWinLevel);
            showWinMenuUIEvent.Fire();
            stopMilitaryBuildingsEvent.Fire();
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
            stopMilitaryBuildingsEvent.Fire();
            enemySpawner.StopEnemies();
        }

        public void RestartLevel(PlayerHasRestartedLevelEvent levelEvent)
        {
            _sceneChanger.RestartScene(levelEvent);
        }

        public void WinLevel(PlayerHasWonLevelEvent levelEvent)
        {
            EnemyHasBeenDefeated(null);
        }

        public void LostLevel(PlayerHasLostLevelEvent levelEvent)
        {
            LostLogic();
        }
    }
}