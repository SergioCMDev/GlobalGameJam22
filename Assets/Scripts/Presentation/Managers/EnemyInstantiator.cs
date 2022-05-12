
using System;
using System.Collections.Generic;
using App;
using Presentation.Events;
using Presentation.Hostiles;
using Presentation.Infrastructure;
using UnityEngine;

namespace Presentation.Managers
{
    public class EnemyInstantiator : MonoBehaviour
    {

        [SerializeField] private GridBuildingManager gridBuildingManager;
        [SerializeField] private GridMovementManager gridMovementManager;
        public event Action<Enemy> OnEnemyHasBeenDefeated;

        [SerializeField] private bool instantiate;
        [SerializeField] private Vector3Int positionToInstantiate;
        [SerializeField] private GameObject enemyPrefab;

        private List<Building> _citiesToDestroy;
        private readonly List<Enemy> _activeEnemies = new List<Enemy>();

        private void Start()
        {
            if (!instantiate) return;

            InstantiateEnemy(enemyPrefab, positionToInstantiate, 100, 0.5f);
        }

        public void SetCitiesToDestroy(List<Building> cityBuilding1)
        {
            _citiesToDestroy = cityBuilding1;
        }
        public void InstantiateEnemy(InstantiateEnemyEvent instantiateEnemyEvent)
        {
            InstantiateEnemy(instantiateEnemyEvent.Prefab, instantiateEnemyEvent.PositionToInstantiate,
                instantiateEnemyEvent.Life, instantiateEnemyEvent.Speed);
        }

        //TODO USE ScriptableObjects to life and speed
        private void InstantiateEnemy(GameObject enemyPrefab, Vector3Int positionToInstantiate, float life, float speed)
        {
            if (!gridBuildingManager.WorldTileDictionary.ContainsKey(positionToInstantiate))
                return;

            var positionToPutEnemy = gridBuildingManager
                .WorldTileDictionary[positionToInstantiate].WorldPosition;
            var enemyInstance = Instantiate(enemyPrefab, positionToPutEnemy, Quaternion.identity);
            var enemy = enemyInstance.GetComponent<Enemy>();
            GridPathfinding gridPathfinding = new GridPathfinding();
            gridPathfinding.Init(gridBuildingManager.WorldTileDictionary, enemy.TilesToFollow);
            
            enemy.Init(positionToInstantiate, _citiesToDestroy, gridPathfinding, life, speed);
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


    }
}