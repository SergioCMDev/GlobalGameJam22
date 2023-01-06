using System.Collections.Generic;
using App.Events;
using Presentation.Events;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.Managers;
using Presentation.UI.Menus;
using Services.GameData;
using Services.ScenesChanger;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class GameStatusController : MonoBehaviour
    {
        [SerializeField] private List<BuildingPositionTuple> buildingPositionTuples;
        [SerializeField] private ShowWinMenuUIEvent showWinMenuUIEvent;
        [SerializeField] private ShowLostMenuUIEvent showLostMenuUIEvent;
        [SerializeField] private DeactivateMilitaryBuildingsEvent deactivateMilitaryBuildingsEvent;
        [SerializeField] private DeactivateUISlidersEvent deactivateUISlidersEvent;
        [SerializeField] private ActivateMilitaryBuildingsEvent activateMilitaryBuildingsEvent;

        [SerializeField] private bool startGame = false;
        [Space] [Header("EnemySpawner")] [SerializeField]
        private GridBuildingManager gridBuildingManager;

        [SerializeField] private Vector3Int positionToInstantiate = new(1, -4, 0);
        [SerializeField] private Transform enemiesParent;
        [SerializeField] private bool instantiate;

        [Space] [Header("RoundsController")] 
        [SerializeField] private CanvasPresenter canvasPresenter;

        [SerializeField] private int numberOfRoundsPerLevel;
        [SerializeField] private float timeToDefendAgainstSlimes = 20, timeToAllowPlayerBuildsTurrets;
        [SerializeField] private float timeToShowNewRoundPopup = 3;

        [Space] [Header("GridMovementManager")] 
        [SerializeField] private Grid grid;


        // [SerializeField] private ObjectHasMovedToNewTileEvent _eventMovement;
        // [SerializeField] private TestMovement testMovement;


        private GridMovementManager gridMovementManager;
        private RoundsController roundsController;
        private EnemySpawner enemySpawner;
        private SceneChangerService _sceneChangerService;
        private GameDataService _gameDataService;
        // private SoundPlayer _soundPlayer;
        private float _remainingTimeToWin;
        private bool _timerIsRunning;
        private readonly List<Building> _buildings = new();

        private void Awake()
        {
            gridMovementManager = new GridMovementManager();

            foreach (var VARIABLE in buildingPositionTuples)
            {
                _buildings.Add(VARIABLE.cityBuilding);
            }

            enemySpawner = new EnemySpawner();

            roundsController = new RoundsController();
            
            enemySpawner.SetCitiesToDestroy(_buildings);
            roundsController.OnPlayerHasBeenDefeated += LostLogic;
            roundsController.OnActivateMilitaryBuildings += activateMilitaryBuildingsEvent.Fire;
            roundsController.OnDeactivateMilitaryBuildings += deactivateMilitaryBuildingsEvent.Fire;
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
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            // _soundPlayer = ServiceLocator.Instance.GetService<SoundPlayer>();
            _gameDataService = ServiceLocator.Instance.GetService<GameDataService>();

            enemySpawner.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;
            
            foreach (var cityBuilding in _buildings)
            {
                cityBuilding.OnBuildingDestroyed += CityHasBeenDestroyed;
                cityBuilding.Initialize();
            }
            
            gridMovementManager.Init(new GridMovementManagerInitData()
            {
                gridBuildingManager = gridBuildingManager,
                grid = grid
            });

            enemySpawner.Init(new EnemySpawnerInitData()
            {
                Instantiate = instantiate,
                GridBuildingManager = gridBuildingManager,
                GridMovementManager = gridMovementManager,
                EnemiesParent = enemiesParent,
                PositionToInstantiate = positionToInstantiate
            });
            
            
            roundsController.Init(new RoundsController.RoundsControllerInitData()
            {
                CanvasPresenter = canvasPresenter,
                EnemySpawner = enemySpawner,
                NumberOfRoundsPerLevel = numberOfRoundsPerLevel,
                TimeToDefendAgainstSlimes = timeToDefendAgainstSlimes,
                TimeToShowNewRoundPopup = timeToShowNewRoundPopup,
                TimeToAllowPlayerBuildsTurrets = timeToAllowPlayerBuildsTurrets
            });
            
            gridBuildingManager.SetCitiesInGrid(buildingPositionTuples);
            if (!startGame) return;
            roundsController.StartNewRound();
        }


        private void CityHasBeenDestroyed(Building building)
        {
            _buildings.Remove(building);
            if (_buildings.Count != 0) return;
            LostLogic();
        }

        private void LostLogic()
        {
            // _soundPlayer.PlaySfx(SfxSoundName.PlayerLoseLevel);
            showLostMenuUIEvent.Fire();

            StopGameCommonLogic();
        }

        public void RestartLevel(PlayerHasRestartedLevelEvent levelEvent)
        {
            _sceneChangerService.RestartScene(levelEvent);
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

            // _soundPlayer.PlaySfx(SfxSoundName.PlayerWinLevel);
            _gameDataService.SaveGame(_sceneChangerService.GetCurrentSceneName());
        }

        private void StopGameCommonLogic()
        {
            enemySpawner.StopEnemies();
            deactivateMilitaryBuildingsEvent.Fire();
            deactivateUISlidersEvent.Fire();
        }

        public void SpawnEnemy(SpawnEnemyEvent spawnEnemyEvent)
        {
            enemySpawner.SpawnEnemy(spawnEnemyEvent.enemyInfo, spawnEnemyEvent.positionToInstantiate);
        }
    }
}