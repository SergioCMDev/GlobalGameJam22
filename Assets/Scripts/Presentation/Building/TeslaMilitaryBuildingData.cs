using UnityEngine;

namespace Presentation.Building
{
    [CreateAssetMenu(fileName = "TeslaAttackBehaviourData", menuName = "Turrets/AttackBehaviourData/TeslaAttackBehaviourData")]
    public class TeslaMilitaryBuildingData : MilitaryBuildingData
    {
        public float percentageToReduceSpeed, durationOfEffect;
    }
}