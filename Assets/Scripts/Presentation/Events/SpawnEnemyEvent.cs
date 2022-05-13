using App.Events;
using Presentation.Managers;
using UnityEngine;

namespace Presentation.Events
{
    [CreateAssetMenu(fileName = "InstantiateEnemyEvent",
        menuName = "Events/Enemy/InstantiateEnemyEvent")]
    public class SpawnEnemyEvent : GameEventScriptable
    {
        public EnemyInfo enemyInfo;
        public GameObject enemyPrefab;
        public Vector3Int positionToInstantiate;
    }
}