using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Managers
{
    [CreateAssetMenu(menuName = "Enemy/ListForLevel", fileName = "EnemyListForLevelData")]
    public class EnemyListForLevelData : ScriptableObject
    {
        public List<EnemyListForLevel> EnemyListForLevels;
    }
}