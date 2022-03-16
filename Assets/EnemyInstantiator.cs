using Presentation;
using Presentation.Building;
using Presentation.Hostiles;
using UnityEngine;

public class EnemyInstantiator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private CityBuilding cityBuilding;
    [SerializeField] private GridBuildingManager gridBuildingManager;
    [SerializeField] private bool instantiate;
    private GameObject enemyInstance;

    private Enemy enemy;

    void Start()
    {
        if (!instantiate) return;
        enemyInstance = Instantiate(enemyPrefab);
        enemy = enemyInstance.GetComponent<Enemy>();
        GridPathfinding gridPathfinding = new GridPathfinding();
        gridPathfinding.Init(gridBuildingManager.WorldTileDictionary);
        enemy.Init(Vector3Int.zero, cityBuilding, gridPathfinding);
    }
}