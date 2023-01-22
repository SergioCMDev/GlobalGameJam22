using System.Collections.Generic;
using App.Events;
using App.Resources;
using Presentation.Events;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.Managers;
using Presentation.UI;
using Presentation.UI.Menus;
using Services.GameData;
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
        [SerializeField] private List<BuildingPositionTuple> buildingPositionTuples;
        [SerializeField] private ShowWinMenuUIEvent showWinMenuUIEvent;
        [SerializeField] private ShowLostMenuUIEvent showLostMenuUIEvent;
        [SerializeField] private DeactivateMilitaryBuildingsEvent deactivateMilitaryBuildingsEvent;
        [SerializeField] private DeactivateUISlidersEvent deactivateUISlidersEvent;
        [SerializeField] private ActivateMilitaryBuildingsEvent activateMilitaryBuildingsEvent;
        [SerializeField] private ChangeToSpecificSceneEvent changeToSpecificSceneEvent;
        
        [SerializeField] private bool startGame = false;


        [SerializeField] private Vector3Int positionToInstantiate = new(1, -4, 0);
        [SerializeField] private Transform enemiesParent;
        [SerializeField] private bool instantiate;

        [Space] [Header("RoundsController")] [SerializeField]
        private CanvasPresenter canvasPresenter;
        [SerializeField] private SliderLogic sliderLogic;
        [SerializeField] private int numberOfRoundsPerLevel;
        [SerializeField] private float timeToDefendAgainstSlimes = 20, timeToAllowPlayerBuildsTurrets;
        [SerializeField] private float timeToShowNewRoundPopup = 3;
        
        [Space] [Header("GridBuildingManager")]
        [SerializeField] private Grid _grid;
        
        [SerializeField] private SaveBuildingEvent saveBuildingEvent;
        
        [SerializeField] private GridBuildingManager _gridBuildingManager;
        private GridMovementManager _gridMovementManager;
        private RoundsController _roundsController;
        private EnemySpawner _enemySpawner;
        private SceneChangerService _sceneChangerService;
        private PopupGenerator _popupGenerator;
        private ResourcesManagerService _resourcesManagerService;

        private GameDataService _gameDataService;

        // private SoundPlayer _soundPlayer;
        private float _remainingTimeToWin;
        private bool _timerIsRunning;
        private readonly List<Building> _buildings = new();
        private IGameStatusModel _gameStatusModel;
        public GridBuildingManager GridBuildingManager => _gridBuildingManager;

        private void Awake()
        {
            _gridMovementManager = new GridMovementManager();

            foreach (var VARIABLE in buildingPositionTuples)
            {
                _buildings.Add(VARIABLE.cityBuilding);
            }

            _enemySpawner = new EnemySpawner();

            _roundsController = new RoundsController();

            _enemySpawner.SetCitiesToDestroy(_buildings);
            _roundsController.OnPlayerHasBeenDefeated += LostLogic;
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
            _roundsController.OnPlayerHasBeenDefeated -= LostLogic;
            _enemySpawner.OnEnemyHasBeenDefeated -= EnemyHasBeenDefeated;
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
            _resourcesManagerService = ServiceLocator.Instance.GetService<ResourcesManagerService>();
            _popupGenerator = ServiceLocator.Instance.GetService<PopupGenerator>();
            _gameStatusModel = ServiceLocator.Instance.GetModel<IGameStatusModel>();

            _enemySpawner.OnEnemyHasBeenDefeated += EnemyHasBeenDefeated;

            foreach (var cityBuilding in _buildings)
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
                PositionToInstantiate = positionToInstantiate
            });

            _roundsController.Init(new RoundsController.RoundsControllerInitData()
            {
                CanvasPresenter = canvasPresenter,
                SliderLogic = sliderLogic,
                EnemySpawner = _enemySpawner,
                NumberOfRoundsPerLevel = numberOfRoundsPerLevel,
                TimeToDefendAgainstSlimes = timeToDefendAgainstSlimes,
                TimeToShowNewRoundPopup = timeToShowNewRoundPopup,
                TimeToAllowPlayerBuildsTurrets = timeToAllowPlayerBuildsTurrets
            });

            _gridBuildingManager.SetCitiesInGrid(buildingPositionTuples);
            _resourcesManagerService.OverrideResources(RetrievableResourceType.Gold, _resourcesManagerService.GetInitialGoldByLevel(_sceneChangerService.GetCurrentSceneName()));
            _popupGenerator.UpdateCamera();
            if (_gameStatusModel.GameStatus != GameStatus.RESTARTING) return;
            InitGame();
            AddFaderScene();
        }

        private void AddFaderScene()
        {
            if (Utilities.SceneIsLoaded(_sceneChangerService.GetFaderSceneName())) return;
            SceneManager.LoadSceneAsync(_sceneChangerService.GetFaderSceneName(), LoadSceneMode.Additive);
        }

        private void InitGame()
        {
            if (!startGame) return;
            _roundsController.StartNewRound();
        }


        private void CityHasBeenDestroyed(Building building)
        {
            _buildings.Remove(building);
            if (_buildings.Count != 0) return;
            LostLogic();
        }

        private void LostLogic()
        {
            _roundsController.OnPlayerHasBeenDefeated -= LostLogic;

            // _soundPlayer.PlaySfx(SfxSoundName.PlayerLoseLevel);
            showLostMenuUIEvent.Fire();

            StopGameCommonLogic();
        }

        public void RestartLevel(PlayerHasRestartedLevelEvent _)
        {
            Time.timeScale = 1;
            _gameStatusModel.GameStatus = GameStatus.RESTARTING;
            _sceneChangerService.RestartScene();
            _resourcesManagerService.OverrideResources(RetrievableResourceType.Gold, _resourcesManagerService.GetInitialGoldByLevel(_sceneChangerService.GetCurrentSceneName()));
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
            EnemyHasBeenDefeated();
        }

        public void LostLevel(PlayerHasLostLevelEvent levelEvent)
        {
            LostLogic();
        }

        private void EnemyHasBeenDefeated(Enemy enemy = null)
        {
            _enemySpawner.OnEnemyHasBeenDefeated -= EnemyHasBeenDefeated;
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
            _enemySpawner.SpawnEnemy(spawnEnemyEvent.enemyInfo, spawnEnemyEvent.positionToInstantiate);
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
    }
}