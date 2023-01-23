using System;
using System.Collections.Generic;
using App;
using App.Info.Enemies;
using App.Info.Tuples;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Services.EnemySpawner;
using UnityEngine;
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
        private List<Enemy> _activeEnemies;
        private Dictionary<int, EnemyType> _enemyListForLevel;

        public void Init(EnemySpawnerInitData enemySpawnerInitData)
        {
            _enemySpawnerService = ServiceLocator.Instance.GetService<EnemySpawnerService>();
            gridBuildingManager = enemySpawnerInitData.GridBuildingManager;
            instantiate = enemySpawnerInitData.Instantiate;
            positionToInstantiate = enemySpawnerInitData.PositionToInstantiate;
            enemiesParent = enemySpawnerInitData.EnemiesParent;
            gridMovementManager = enemySpawnerInitData.GridMovementManager;
            _activeEnemies = new List<Enemy>();
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
        public void SpawnEnemy(EnemySpawnerInfo enemySpawnerInfo, Vector3Int positionToInstantiate)
        {
            if (!gridBuildingManager.WorldTileDictionary.ContainsKey(positionToInstantiate))
                return;

            var positionToPutEnemy = gridBuildingManager
                .WorldTileDictionary[positionToInstantiate].WorldPosition;
            var enemyInstance = GameObject.Instantiate(enemySpawnerInfo.enemyPrefab, positionToPutEnemy,
                Quaternion.identity, enemiesParent);
            var enemy = enemyInstance.GetComponent<Enemy>();
            var gridPathfinding = new GridPathfinding();
            gridPathfinding.Init(gridBuildingManager.WorldTileDictionary, enemySpawnerInfo.TilesToFollow);

            enemy.Init(positionToInstantiate, _citiesToDestroy, gridPathfinding, enemySpawnerInfo);
            enemy.OnEnemyHasBeenDefeated += EnemyDefeated;
            _activeEnemies.Add(enemy);
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
            _activeEnemies.Remove(enemy);

            if (_activeEnemies.Count == 0)
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
            if (round < _enemyListForLevel.Count)
            {
                enemyType = _enemyListForLevel[round];
            }

            var enemySpawnerInfo = _enemySpawnerService.GetEnemyPrefabByType(enemyType);
            SpawnEnemy(enemySpawnerInfo, positionToInstantiate);
        }

        public void HideEnemies()
        {
            foreach (Transform enemy in enemiesParent.transform)
            {
                GameObject.Destroy(enemy.gameObject);
            }
        }
    }
}