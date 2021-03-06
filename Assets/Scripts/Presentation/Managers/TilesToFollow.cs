using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Presentation.Managers
{
    [CreateAssetMenu(fileName = "EnemyPathFinding", menuName = "Enemy/Pathfinding")]
    public class TilesToFollow : ScriptableObject
    {
        [FormerlySerializedAs("TilePositions")] public List<TilePosition> tilePositions;
    }
}