using System;
using App.Events;
using App.SceneManagement;
using Presentation.Building;
using Presentation.Hostiles;
using Presentation.Managers;
using Presentation.Menus;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class GameStatusController : MonoBehaviour
    {
        [SerializeField] private CityBuilding _cityBuilding;
        [SerializeField] private SliderBarView _sliderBarView;
        [SerializeField] private EnemyInstantiator enemyInstantiator;

        [SerializeField] private ShowWinMenuUIEvent showWinMenuUIEvent;
        [SerializeField] private ShowLostMenuUIEvent showLostMenuUIEvent;
        [SerializeField] private StopMilitaryBuildingsEvent stopMilitaryBuildingsEvent;
        [SerializeField] private float _timeToWin = 20, _timeToAllowPlayerBuildsTurrets;
        [SerializeField] private bool _skipTimer;
        private SceneChanger _sceneChanger;
        private SoundManager _soundManager;
        private float _remainingTimeToWin;
        private bool _timerIsRunning;

        void Start()
        {
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
            _soundManager = ServiceLocator.Instance.GetService<SoundManager>();
            enemyInstantiator.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;

            _cityBuilding.OnBuildingDestroyed += PlayerHasBeenDefeated;
            _sliderBarView.SetMaxValue(_timeToWin);
            _sliderBarView.OnSliderReachZero += TimeHasEnded;
            _remainingTimeToWin = _timeToWin;
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
        }

        private void PlayerHasBeenDefeated(Building.Building building)
        {
            _soundManager.PlaySfx(SfxSoundName.PlayerLoseLevel);
            showLostMenuUIEvent.Fire();
        }

        public void RestartLevel(PlayerHasRestartedLevelEvent levelEvent)
        {
            _sceneChanger.RestartScene(levelEvent);
        }
    }
}