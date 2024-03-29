using System;
using System.Collections.Generic;
using App;
using App.Info.Enemies;
using App.Info.Tiles;
using App.Info.Tuples;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Services.EnemySpawner;
using Services.PathRetriever;
using Services.ScenesChanger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation.Managers
{
    public struct EnemySpawnerInitData
    {
        public GridBuildingManager GridBuildingManager;
        public GridMovementManager GridMovementManager;
        public bool Instantiate;
        public Vector3Int PositionToInstantiate;
        public Transform EnemiesParent;
        public List<EnemyListForLevel> EnemyListForLevel;
    }

    [Serializable]
    public struct EnemyListForLevel
    {
        public int roundToAppear;
        public EnemyType enemyType;
    }

    public class EnemySpawner
    {
        private GridBuildingManager gridBuildingManager;
        private GridMovementManager gridMovementManager;
        private bool instantiate;
        private Vector3Int positionToInstantiate;
        private Transform enemiesParent;
        public event Action OnEnemiesHaveBeenDefeated;


        private List<City> _citiesToDestroy;
        private EnemySpawnerService _enemySpawnerService;
        private PathRetrieverService _pathRetrieverService;
        private int _activeEnemies;
        private Dictionary<int, EnemyType> _enemyListForLevel;
        private SceneChangerService _sceneChangerService;

        public void Init(EnemySpawnerInitData enemySpawnerInitData)
        {
            _enemySpawnerService = ServiceLocator.Instance.GetService<EnemySpawnerService>();
            _pathRetrieverService = ServiceLocator.Instance.GetService<PathRetrieverService>();
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            gridBuildingManager = enemySpawnerInitData.GridBuildingManager;
            instantiate = enemySpawnerInitData.Instantiate;
            positionToInstantiate = enemySpawnerInitData.PositionToInstantiate;
            enemiesParent = enemySpawnerInitData.EnemiesParent;
            gridMovementManager = enemySpawnerInitData.GridMovementManager;
            _activeEnemies = 0;
            _enemyListForLevel = new Dictionary<int, EnemyType>();
            foreach (var VARIABLE in enemySpawnerInitData.EnemyListForLevel)
            {
                _enemyListForLevel.Add(VARIABLE.roundToAppear, VARIABLE.enemyType);
            }
        }

        public void SetCitiesToDestroy(List<City> cityBuilding1)
        {
            _citiesToDestroy = cityBuilding1;
        }

        //TODO USE ScriptableObjects to life and speed
        public void SpawnEnemy(EnemySpawnerInfo enemySpawnerInfo, Vector3Int positionToInstantiate, List<TilePosition> tilesToFollows)
        {
            if (!gridBuildingManager.WorldTileDictionary.ContainsKey(positionToInstantiate))
                return;

            var positionToPutEnemy = gridBuildingManager
                .WorldTileDictionary[positionToInstantiate].WorldPosition;
            var enemyInstance = GameObject.Instantiate(enemySpawnerInfo.enemyPrefab, positionToPutEnemy,
                Quaternion.identity, enemiesParent);
            var enemy = enemyInstance.GetComponent<Enemy>();
            var gridPathfinding = new GridPathfinding();
            gridPathfinding.Init(gridBuildingManager.WorldTileDictionary, tilesToFollows);

            enemy.Init(positionToInstantiate, _citiesToDestroy, gridPathfinding, enemySpawnerInfo);
            enemy.OnEnemyHasBeenDefeated += EnemyDefeated;
            _activeEnemies++;
            enemy.OnObjectMoved += OnObjectMoved;
        }

        private void OnObjectMoved(GameObject movable, WorldPositionTuple worldPosition)
        {
            gridMovementManager.OnObjectMoved(movable, worldPosition);
        }

        private void EnemyDefeated(Enemy enemy)
        {
            enemy.OnEnemyHasBeenDefeated -= EnemyDefeated;
            enemy.OnObjectMoved -= OnObjectMoved;
            _activeEnemies--;
            HideEnemy(enemy.gameObject);
            if (_activeEnemies == 0)
            {
                OnEnemiesHaveBeenDefeated?.Invoke();
            }
        }

        public void StopEnemies()
        {
            foreach (Transform enemy in enemiesParent.transform)
            {
                enemy.GetComponent<Enemy>().Deactivate();
            }

            Debug.Log("enemies have been stopped");
        }

        public void ActivateEnemiesByTimer(int round)
        {
            if (!instantiate) return;
            var enemyType = EnemyType.Normal;
            if (round <= _enemyListForLevel.Count)
            {
                enemyType = _enemyListForLevel[round];
            }

            var enemySpawnerInfo = _enemySpawnerService.GetEnemyPrefabByType(enemyType);
            var currentSceneName = _sceneChangerService.GetCurrentSceneName();
            var tilesToFollow = _pathRetrieverService.GetTilesToFollowByEnemyAndLevel(enemyType, currentSceneName);
            SpawnEnemy(enemySpawnerInfo, positionToInstantiate, tilesToFollow);
        }

        public void HideEnemies()
        {
            foreach (Transform enemy in enemiesParent.transform)
            {
                _activeEnemies--;

                HideEnemy(enemy.gameObject);
            }
        }

        private void HideEnemy(GameObject enemyGameObject)
        {
            GameObject.Destroy(enemyGameObject);
        }
    }
}