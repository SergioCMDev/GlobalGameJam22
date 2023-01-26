using System;
using System.Collections.Generic;
using App.Events;
using App.Resources;
using Presentation.Events;
using Presentation.Infrastructure;
using Presentation.Managers;
using Presentation.UI;
using Presentation.UI.Menus;
using Services.GameData;
using Services.PathRetriever;
using Services.Popups;
using Services.ResourcesManager;
using Services.ScenesChanger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation
{
    public class GameStatusController : MonoBehaviour
    {
        [SerializeField] private List<CitiesPositionTuple> buildingPositionTuples;
        [SerializeField] private ShowWinMenuUIEvent showWinMenuUIEvent;
        [SerializeField] private ShowLostMenuUIEvent showLostMenuUIEvent;
        [SerializeField] private DeactivateMilitaryBuildingsEvent deactivateMilitaryBuildingsEvent;
        [SerializeField] private DeactivateUISlidersEvent deactivateUISlidersEvent;
        [SerializeField] private ActivateMilitaryBuildingsEvent activateMilitaryBuildingsEvent;
        [SerializeField] private ChangeToSpecificSceneEvent changeToSpecificSceneEvent;

        [SerializeField] private bool startGame = false;
        [SerializeField] private EnemyListForLevelData enemyListForLevels;

        [SerializeField] private Vector3Int positionToInstantiate = new(1, -4, 0);
        [SerializeField] private Transform enemiesParent;
        [SerializeField] private bool instantiate;

        [Space] [Header("RoundsController")] [SerializeField]
        private CanvasPresenter canvasPresenter;

        [SerializeField] private SliderLogic sliderLogic;
        [SerializeField] private int numberOfRoundsPerLevel;
        [SerializeField] private float timeToAllowPlayerBuildsTurrets;
        [SerializeField] private float timeToShowNewRoundPopup = 3;

        [Space] [Header("GridBuildingManager")] [SerializeField]
        private Grid _grid;

        [SerializeField] private SaveBuildingEvent saveBuildingEvent;

        [SerializeField] private GridBuildingManager _gridBuildingManager;
        private GridMovementManager _gridMovementManager;
        private RoundsController _roundsController;
        private EnemySpawner _enemySpawner;
        private SceneChangerService _sceneChangerService;
        private ResourcesManagerService _resourcesManagerService;
        private PathRetrieverService _pathRetrieverService;

        private GameDataService _gameDataService;

        // private SoundPlayer _soundPlayer;
        private float _remainingTimeToWin;
        private bool _timerIsRunning;
        private readonly List<City> _cities = new();
        private IGameStatusModel _gameStatusModel;
        public GridBuildingManager GridBuildingManager => _gridBuildingManager;

        private void Awake()
        {
            _gridMovementManager = new GridMovementManager();

            foreach (var VARIABLE in buildingPositionTuples)
            {
                _cities.Add(VARIABLE.cityBuilding);
            }

            _enemySpawner = new EnemySpawner();

            _roundsController = new RoundsController();
            _enemySpawner.SetCitiesToDestroy(_cities);
            _roundsController.OnActivateMilitaryBuildings += activateMilitaryBuildingsEvent.Fire;
            _roundsController.OnDeactivateMilitaryBuildings += deactivateMilitaryBuildingsEvent.Fire;
        }

        private void ThrowSaveBuilding(GameObject obj)
        {
            _gridBuildingManager.OnSaveBuilding -= ThrowSaveBuilding;

            saveBuildingEvent.Instance = obj;
            saveBuildingEvent.Fire();
        }

        private void OnDestroy()
        {
            _roundsController.OnPlayerHasWon -= PlayerHasDefeatedEnemies;
            _roundsController.OnActivateMilitaryBuildings -= activateMilitaryBuildingsEvent.Fire;
            _roundsController.OnDeactivateMilitaryBuildings -= deactivateMilitaryBuildingsEvent.Fire;

            foreach (var cityBuilding in _cities)
            {
                cityBuilding.OnBuildingDestroyed -= CityHasBeenDestroyed;
            }
        }

        void Start()
        {
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            _pathRetrieverService = ServiceLocator.Instance.GetService<PathRetrieverService>();

            // _soundPlayer = ServiceLocator.Instance.GetService<SoundPlayer>();
            _gameDataService = ServiceLocator.Instance.GetService<GameDataService>();
            _resourcesManagerService = ServiceLocator.Instance.GetService<ResourcesManagerService>();
            _gameStatusModel = ServiceLocator.Instance.GetModel<IGameStatusModel>();

            foreach (var cityBuilding in _cities)
            {
                cityBuilding.OnBuildingDestroyed += CityHasBeenDestroyed;
                cityBuilding.Initialize();
            }

            _gridBuildingManager.Init();

            _gridMovementManager.Init(new GridMovementManagerInitData()
            {
                gridBuildingManager = _gridBuildingManager,
                grid = _grid
            });

            _enemySpawner.Init(new EnemySpawnerInitData()
            {
                Instantiate = instantiate,
                GridBuildingManager = _gridBuildingManager,
                GridMovementManager = _gridMovementManager,
                EnemiesParent = enemiesParent,
                PositionToInstantiate = positionToInstantiate,
                EnemyListForLevel = enemyListForLevels.EnemyListForLevels
            });

            _roundsController.Init(new RoundsController.RoundsControllerInitData()
            {
                CanvasPresenter = canvasPresenter,
                SliderLogic = sliderLogic,
                EnemySpawner = _enemySpawner,
                NumberOfRoundsPerLevel = numberOfRoundsPerLevel,
                TimeToShowNewRoundPopup = timeToShowNewRoundPopup,
                TimeToAllowPlayerBuildsTurrets = timeToAllowPlayerBuildsTurrets
            });

            _gridBuildingManager.SetCitiesInGrid(buildingPositionTuples);
            _resourcesManagerService.OverrideResources(RetrievableResourceType.Gold,
                _resourcesManagerService.GetInitialGoldByLevel(_sceneChangerService.GetCurrentSceneName()));
            CheckGameStatus();
        }

        private void CheckGameStatus()
        {
            switch (_gameStatusModel.GameStatus)
            {
                case GameStatus.STARTING_FROM_SAME_SCENE:
                case GameStatus.RESTARTING:
                    InitGame();
                    AddFaderScene();
                    break;
                case GameStatus.STARTING_FROM_MENU:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddFaderScene()
        {
            SceneManager.LoadSceneAsync(_sceneChangerService.GetFaderSceneName(), LoadSceneMode.Additive);
        }

        private void InitGame()
        {
            if (!startGame) return;
            _roundsController.StartNewRound();
        }

        private void CityHasBeenDestroyed(City building)
        {
            building.OnBuildingDestroyed -= CityHasBeenDestroyed;

            _cities.Remove(building);
            if (_cities.Count != 0) return;
            LostLogic();
        }

        private void LostLogic()
        {
            // _soundPlayer.PlaySfx(SfxSoundName.PlayerLoseLevel);
            showLostMenuUIEvent.Fire();

            StopGameCommonLogic();
        }

        public void RestartLevel(PlayerHasRestartedLevelEvent _)
        {
            Time.timeScale = 1;
            _gameStatusModel.GameStatus = GameStatus.RESTARTING;
            _sceneChangerService.RestartScene();
            _resourcesManagerService.OverrideResources(RetrievableResourceType.Gold,
                _resourcesManagerService.GetInitialGoldByLevel(_sceneChangerService.GetCurrentSceneName()));
        }

        public void ChangeToNextScene(ChangeToNextSceneEvent _)
        {
            var sceneName = _sceneChangerService.GetNextSceneFromCurrent();
            changeToSpecificSceneEvent.SceneName = sceneName;
            changeToSpecificSceneEvent.Fire();
        }

        public void FadeDisappearedEvent(FadeDisappearedEvent _)
        {
            InitGame();
        }

        public void ExitedLevel(PlayerHasExitedLevelEvent _)
        {
            Time.timeScale = 1;
            changeToSpecificSceneEvent.SceneName = _sceneChangerService.GetMainMenuSceneName();
            changeToSpecificSceneEvent.Fire();
        }

        public void WinLevel(PlayerHasWonLevelEvent levelEvent)
        {
            PlayerHasDefeatedEnemies();
        }

        public void LostLevel(PlayerHasLostLevelEvent levelEvent)
        {
            LostLogic();
        }

        private void PlayerHasDefeatedEnemies()
        {
            _roundsController.OnPlayerHasWon -= PlayerHasDefeatedEnemies;
            showWinMenuUIEvent.Fire();

            StopGameCommonLogic();

            // _soundPlayer.PlaySfx(SfxSoundName.PlayerWinLevel);
            _gameDataService.SaveGame(_sceneChangerService.GetCurrentSceneName());
        }

        private void StopGameCommonLogic()
        {
            _enemySpawner.StopEnemies();
            deactivateMilitaryBuildingsEvent.Fire();
            deactivateUISlidersEvent.Fire();
        }

        public void SpawnEnemy(SpawnEnemyEvent spawnEnemyEvent)
        {
            var tilesToFollow = _pathRetrieverService.GetTilesToFollowByEnemyAndLevel(
                spawnEnemyEvent.enemyInfo.enemyType,
                _sceneChangerService.GetCurrentSceneName());
            _enemySpawner.SpawnEnemy(spawnEnemyEvent.enemyInfo, spawnEnemyEvent.positionToInstantiate, tilesToFollow);
        }

        //Used by ShowRange Button
        public void StatusDrawingTurretRange(SetStatusDrawingTurretRangesEvent setStatusDrawingTurretRangesEvent)
        {
            _gridBuildingManager.StatusDrawingTurretRange(setStatusDrawingTurretRangesEvent.drawingStatus);
        }

        public void AllowPlayerToSetBuildingInTilemap(AllowPlayerToSetBuildingInTilemapEvent tilemapEvent)
        {
            _gridBuildingManager.OnSaveBuilding += ThrowSaveBuilding;
            _gridBuildingManager.AllowPlayerToSetBuildingInTilemap(tilemapEvent.Prefab,
                tilemapEvent.militaryBuildingType);
        }

        public void ObjectHasMovedToNewTile(ObjectHasMovedToNewTileEvent tileEvent)
        {
            _gridBuildingManager.ObjectHasMovedToNewTile(tileEvent.Occupier, tileEvent.GridPositions);
        }

        public void SkipRound()
        {
            _roundsController.SkipRound();
        }
    }
}