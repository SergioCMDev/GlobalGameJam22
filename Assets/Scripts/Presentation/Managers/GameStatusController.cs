using System.Collections.Generic;
using App.Events;
using App.SceneManagement;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.UI.Menus;
using UnityEngine;
using Utils;

namespace Presentation.Managers
{
    public class GameStatusController : MonoBehaviour
    {
        [SerializeField] private SliderBarView _sliderBarView;
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
            enemySpawner.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;
            foreach (var cityBuilding in _buildings)
            {
                cityBuilding.OnBuildingDestroyed += CityHasBeenDestroyed;
            }

            _sliderBarView.SetMaxValue(_timeToWin);
            _sliderBarView.OnSliderReachZero += TimeHasEnded;
            _remainingTimeToWin = _timeToWin;
            gridBuildingManager.SetCitiesInGrid(buildingPositionTuples);
            _timerIsRunning = true;
        }

        private void TimeHasEnded()
        {
            _timerIsRunning = false;
            EnemyHasBeenDefeated(null);
        }

        private void Update()
        {
            if (!_timerIsRunning || _skipTimer) return;
            _remainingTimeToWin -= Time.deltaTime;
            _sliderBarView.SetValue(_remainingTimeToWin);
        }

//Refactor
        private void EnemyHasBeenDefeated(Enemy enemy)
        {
            _soundManager.PlaySfx(SfxSoundName.PlayerWinLevel);
            showWinMenuUIEvent.Fire();
            stopMilitaryBuildingsEvent.Fire();
            enemySpawner.StopEnemies();
        }

        private void CityHasBeenDestroyed(Building building)
        {
            _buildings.Remove(building);
            if (_buildings.Count != 0) return;
            _soundManager.PlaySfx(SfxSoundName.PlayerLoseLevel);
            showLostMenuUIEvent.Fire();
            stopMilitaryBuildingsEvent.Fire();
            enemySpawner.StopEnemies();
        }

        public void RestartLevel(PlayerHasRestartedLevelEvent levelEvent)
        {
            _sceneChanger.RestartScene(levelEvent);
        }
    }
}