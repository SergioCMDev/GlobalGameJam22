using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Managers
{
    [CreateAssetMenu(fileName = "EnemyPathFinding", menuName = "Enemy/Pathfinding")]
    public class TilesToFollow : ScriptableObject
    {
        public List<TilePosition> TilePositions;
    }
}