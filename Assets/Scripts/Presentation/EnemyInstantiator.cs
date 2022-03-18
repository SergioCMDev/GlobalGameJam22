using System;
using System.Collections.Generic;
using App;
using Presentation;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using Presentation.Interfaces;
using Presentation.Managers;
using UnityEngine;

namespace Presentation
{
    public class EnemyInstantiator : MonoBehaviour
    {
        [SerializeField] private CityBuilding cityBuilding;

        [SerializeField] private GridBuildingManager gridBuildingManager;
        [SerializeField] private GridMovementManager gridMovementManager;
        public event Action<Enemy> OnEnemyHasBeenDefeated;

        [SerializeField] private bool instantiate;
        [SerializeField] private Vector3Int positionToInstantiate;
        [SerializeField] private GameObject enemyPrefab;

        private List<Enemy> activeEnemies = new List<Enemy>();

        private void Start()
        {
            if (!instantiate) return;

            InstantiateEnemy(enemyPrefab, positionToInstantiate, 100, 0.5f);
        }


        public void InstantiateEnemy(InstantiateEnemyEvent instantiateEnemyEvent)

        {
            InstantiateEnemy(instantiateEnemyEvent.Prefab, instantiateEnemyEvent.PositionToInstantiate,
                instantiateEnemyEvent.Life, instantiateEnemyEvent.Speed);
        }

        public void InstantiateEnemy(GameObject enemyPrefab, Vector3Int positionToInstantiate, float life, float speed)
        {
            if (!gridBuildingManager.WorldTileDictionary.ContainsKey(positionToInstantiate))
                return;

            var positionToPutEnemy = gridBuildingManager
                .WorldTileDictionary[positionToInstantiate].WorldPosition;
            var enemyInstance = Instantiate(enemyPrefab, positionToPutEnemy, Quaternion.identity);
            var enemy = enemyInstance.GetComponent<Enemy>();
            GridPathfinding gridPathfinding = new GridPathfinding();
            gridPathfinding.Init(gridBuildingManager.WorldTileDictionary);
            enemy.Init(positionToInstantiate, cityBuilding, gridPathfinding, life, speed);
            enemy.OnEnemyHasBeenDefeated += EnemyDefeated;
            activeEnemies.Add(enemy);
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
            activeEnemies.Remove(enemy);
        }

        public void StopEnemies()
        {
            foreach (var enemy in activeEnemies)
            {
                enemy.Deactivate();
            }

            Debug.Log("enemies have been stopped");
        }
    }
}