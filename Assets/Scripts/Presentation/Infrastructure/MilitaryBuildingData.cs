using Presentation.Structs;
using UnityEngine;

namespace Presentation.Infrastructure
{
    [CreateAssetMenu(fileName = "AttackBehaviourData", menuName = "Turrets/AttackBehaviourData/Turrets")]
    public class MilitaryBuildingData : ScriptableObject
    {
        public DamageType damageType;
        public float cadence, damage;
    }
}