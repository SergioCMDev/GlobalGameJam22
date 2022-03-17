using System;
using Presentation;
using Presentation.Building;
using Presentation.Hostiles;
using UnityEngine;

namespace Presentation
{
    public class EnemyInstantiator : MonoBehaviour
    {
        // [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private CityBuilding cityBuilding;

        [SerializeField] private GridBuildingManager gridBuildingManager;
        public event Action<Enemy> OnEnemyHasBeenDefeated;

        // [SerializeField] private bool instantiate;
        // [SerializeField] private Vector3Int positionToInstantiate;
        // private GameObject enemyInstance;

        // private Enemy enemy;


        public void InstantiateEnemy(InstantiateEnemyEvent instantiateEnemyEvent)
        {
            if (!gridBuildingManager.WorldTileDictionary.ContainsKey(instantiateEnemyEvent.PositionToInstantiate))
                return;

            var positionToPutEnemy = gridBuildingManager
                .WorldTileDictionary[instantiateEnemyEvent.PositionToInstantiate]
                .WorldPosition;
            var enemyInstance = Instantiate(instantiateEnemyEvent.Prefab, positionToPutEnemy, Quaternion.identity);
            var enemy = enemyInstance.GetComponent<Enemy>();
            GridPathfinding gridPathfinding = new GridPathfinding();
            gridPathfinding.Init(gridBuildingManager.WorldTileDictionary);
            enemy.Init(instantiateEnemyEvent.PositionToInstantiate, cityBuilding, gridPathfinding);
            enemy.OnEnemyHasBeenDefeated += EnemyDefeated;
        }

        private void EnemyDefeated(Enemy enemy)
        {
            enemy.OnEnemyHasBeenDefeated -= EnemyDefeated;
            OnEnemyHasBeenDefeated?.Invoke(enemy);
        }
    }
}