using System;
using UnityEngine;

namespace App.Info.Enemies
{
    [Serializable]
    public class EnemySpawnerInfo
    {
        public EnemyInfo enemyInfo;
        public EnemyType enemyType = EnemyType.Normal;
        public GameObject enemyPrefab;
    }

    public enum EnemyType
    {
        Normal, Red
    }
}