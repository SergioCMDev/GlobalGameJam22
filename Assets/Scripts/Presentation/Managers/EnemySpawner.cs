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
        [SerializeField] private Transform enemiesParent;

        private List<Building> _citiesToDestroy;
        // private readonly List<Enemy> _activeEnemies = new();

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
            var enemyInstance = Instantiate(enemySpawnerInfo.enemyPrefab, positionToPutEnemy, Quaternion.identity, enemiesParent);
            var enemy = enemyInstance.GetComponent<Enemy>();
            GridPathfinding gridPathfinding = new GridPathfinding();
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
            if(!instantiate) return;
            EnemySpawnerInfo enemySpawnerInfo = enemyPrefabs[0];
            SpawnEnemy(enemySpawnerInfo, positionToInstantiate);
        }
        
        public void HideEnemies()
        {
            foreach (Transform enemy in enemiesParent.transform)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}