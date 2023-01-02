using System.Collections.Generic;
using App.Events;
using App.SceneManagement;
using App.Services;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.Managers;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class GameStatusController : MonoBehaviour
    {
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private GridBuildingManager gridBuildingManager;
        [SerializeField] private List<BuildingPositionTuple> buildingPositionTuples;
        [SerializeField] private ShowWinMenuUIEvent showWinMenuUIEvent;
        [SerializeField] private ShowLostMenuUIEvent showLostMenuUIEvent;
        [SerializeField] private RoundsController roundsController;
        [SerializeField] private DeactivateMilitaryBuildingsEvent deactivateMilitaryBuildingsEvent;
        [SerializeField] private DeactivateUISlidersEvent deactivateUISlidersEvent;
        private SceneChanger _sceneChanger;
        private GameDataService _gameDataService;
        private SoundManager _soundManager;
        private float _remainingTimeToWin;
        private bool _timerIsRunning;
        private readonly List<Building> _buildings = new();

        private void Awake()
        {
            foreach (var VARIABLE in buildingPositionTuples)
            {
                _buildings.Add(VARIABLE.cityBuilding);
            }

            enemySpawner.SetCitiesToDestroy(_buildings);
            roundsController.OnPlayerHasBeenDefeated += LostLogic;
        }

        private void OnDestroy()
        {
            roundsController.OnPlayerHasBeenDefeated -= LostLogic;
            enemySpawner.OnEnemyHasBeenDefeated -= EnemyHasBeenDefeated;
            foreach (var cityBuilding in _buildings)
            {
                cityBuilding.OnBuildingDestroyed -= CityHasBeenDestroyed;
            }
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
                cityBuilding.Initialize();
            }

            gridBuildingManager.SetCitiesInGrid(buildingPositionTuples);

            // roundsController.StartNewRound();
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
            
            StopGameCommonLogic();

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

        private void EnemyHasBeenDefeated(Enemy enemy = null)
        {
            enemySpawner.OnEnemyHasBeenDefeated -= EnemyHasBeenDefeated;
            showWinMenuUIEvent.Fire();
            
            StopGameCommonLogic();

            _soundManager.PlaySfx(SfxSoundName.PlayerWinLevel);
            _gameDataService.SaveGame(_sceneChanger.GetCurrentSceneName());
        }

        private void StopGameCommonLogic()
        {
            enemySpawner.StopEnemies();
            deactivateMilitaryBuildingsEvent.Fire();
            deactivateUISlidersEvent.Fire();
        }
    }
}