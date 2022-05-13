using System;
using UnityEngine;

namespace Presentation.Managers
{
    [Serializable]
    public struct EnemySpawnerInfo
    {
        public EnemyInfo enemyInfo;
        public GameObject enemyPrefab;
    }
}