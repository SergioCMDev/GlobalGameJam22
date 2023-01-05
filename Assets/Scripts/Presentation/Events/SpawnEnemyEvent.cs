using App;
using App.Events;
using App.Info.Enemies;
using Presentation.Managers;
using UnityEngine;

namespace Presentation.Events
{
    [CreateAssetMenu(fileName = "InstantiateEnemyEvent",
        menuName = "Events/Enemy/InstantiateEnemyEvent")]
    public class SpawnEnemyEvent : GameEventScriptable
    {
        public EnemySpawnerInfo enemyInfo;
        public Vector3Int positionToInstantiate;
    }
}