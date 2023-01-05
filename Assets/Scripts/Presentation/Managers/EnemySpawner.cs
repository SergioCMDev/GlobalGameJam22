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
    }

    public class EnemySpawner
    {
        private GridBuildingManager gridBuildingManager;
        private GridMovementManager gridMovementManager;
        private bool instantiate;
        private Vector3Int positionToInstantiate;
        private Transform enemiesParent;
        public event Action<Enemy> OnEnemyHasBeenDefeated;


        private List<Building> _citiesToDestroy;
        private EnemySpawnerService _enemySpawnerService;
        
        public void Init(EnemySpawnerInitData enemySpawnerInitData)
        {
            _enemySpawnerService = ServiceLocator.Instance.GetService<EnemySpawnerService>();
            gridBuildingManager = enemySpawnerInitData.GridBuildingManager;
            instantiate = enemySpawnerInitData.Instantiate;
            positionToInstantiate = enemySpawnerInitData.PositionToInstantiate;
            enemiesParent = enemySpawnerInitData.EnemiesParent;
            gridMovementManager = enemySpawnerInitData.GridMovementManager;
        }

        public void SetCitiesToDestroy(List<Building> cityBuilding1)
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
            // _activeEnemies.Add(enemy);
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
            OnEnemyHasBeenDefeated?.Invoke(enemy);
            // _activeEnemies.Remove(enemy);
        }

        public void StopEnemies()
        {
            foreach (Transform enemy in enemiesParent.transform)
            {
                enemy.GetComponent<Enemy>().Deactivate();
            }

            Debug.Log("enemies have been stopped");
        }

        public void ActivateEnemiesByTimer()
        {
            if (!instantiate) return;
            var enemySpawnerInfo = _enemySpawnerService.GetEnemyPrefabByType(EnemyType.Normal);
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