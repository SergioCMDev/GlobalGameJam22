using Application_.Events;
using Application_.SceneManagement;
using Presentation.Building;
using Presentation.Hostiles;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class GameStatusController : MonoBehaviour
    {
        [SerializeField] private CityBuilding _cityBuilding;

        [SerializeField] private ShowWinMenuUIEvent showWinMenuUIEvent;
        [SerializeField] private ShowLostMenuUIEvent showLostMenuUIEvent;
        [SerializeField] private StopMilitaryBuildingsEvent stopMilitaryBuildingsEvent;
        private SceneChanger _sceneChanger;
        private Enemy _enemy;

        void Start()
        {
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
            _enemy = FindObjectOfType<Enemy>();
            if (_enemy)
                _enemy.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;
            _cityBuilding.OnBuildingDestroyed += PlayerHasBeenDefeated;
            
        }


        private void EnemyHasBeenDefeated()
        {
            showWinMenuUIEvent.Fire();
            stopMilitaryBuildingsEvent.Fire();
        }

        private void PlayerHasBeenDefeated(Building.Building building)
        {
            showLostMenuUIEvent.Fire();
        }
        
        public void RestartLevel(PlayerHasRestartedLevelEvent levelEvent)
        {
            _sceneChanger.RestartScene(levelEvent);
        }
    }
}