using System;
using System.Collections.Generic;
using App;
using Presentation.Events;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using UnityEngine;

namespace Presentation.Managers
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GridBuildingManager gridBuildingManager;
        [SerializeField] private GridMovementManager gridMovementManager;
        public event Action<Enemy> OnEnemyHasBeenDefeated;

        [SerializeField] private bool instantiate;
        [SerializeField] private Vector3Int positionToInstantiate;
        [SerializeField] private List<EnemySpawnerInfo> enemyPrefabs;

        private List<Building> _citiesToDestroy;
        private readonly List<Enemy> _activeEnemies = new();

        private void Start()
        {
            if (!instantiate) return;
            ActivateEnemies();
        }

        public void SetCitiesToDestroy(List<Building> cityBuilding1)
        {
            _citiesToDestroy = cityBuilding1;
        }

        public void SpawnEnemy(SpawnEnemyEvent spawnEnemyEvent)
        {
            SpawnEnemy(spawnEnemyEvent.enemyInfo, spawnEnemyEvent.positionToInstantiate);
        }

        //TODO USE ScriptableObjects to life and speed
        private void SpawnEnemy(EnemySpawnerInfo enemySpawnerInfo, Vector3Int positionToInstantiate)
        {
            if (!gridBuildingManager.WorldTileDictionary.ContainsKey(positionToInstantiate))
                return;

            var positionToPutEnemy = gridBuildingManager
                .WorldTileDictionary[positionToInstantiate].WorldPosition;
            var enemyInstance = Instantiate(enemySpawnerInfo.enemyPrefab, positionToPutEnemy, Quaternion.identity);
            var enemy = enemyInstance.GetComponent<Enemy>();
            GridPathfinding gridPathfinding = new GridPathfinding();
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
            OnEnemyHasBeenDefeated?.Invoke(enemy);
            _activeEnemies.Remove(enemy);
        }

        public void StopEnemies()
        {
            foreach (var enemy in _activeEnemies)
            {
                enemy.Deactivate();
            }

            Debug.Log("enemies have been stopped");
        }

        public void ActivateEnemies()
        {
            if(instantiate) return;
            EnemySpawnerInfo enemySpawnerInfo = enemyPrefabs[0];
            SpawnEnemy(enemySpawnerInfo, positionToInstantiate);
        }
    }
}