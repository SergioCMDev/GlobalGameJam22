using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "TeslaAttackBehaviourData", menuName = "Turrets/AttackBehaviourData/TeslaAttackBehaviourData")]
    public class TeslaMilitaryBuildingData : MilitaryBuildingData
    {
        public float percentageToReduceSpeed, durationOfEffect;
    }
}