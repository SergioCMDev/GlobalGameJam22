using UnityEngine;

namespace Presentation.Infrastructure
{
    [CreateAssetMenu(fileName = "TeslaAttackBehaviourData", menuName = "Turrets/AttackBehaviourData/TeslaAttackBehaviourData")]
    public class TeslaMilitaryBuildingData : MilitaryBuildingData
    {
        public float percentageToReduceSpeed, durationOfEffect;
    }
}