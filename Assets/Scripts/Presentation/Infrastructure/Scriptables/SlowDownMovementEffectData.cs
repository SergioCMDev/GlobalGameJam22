using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "TeslaAttackBehaviourData", menuName = "Turrets/AttackBehaviourData/TeslaAttackBehaviourData")]
    public class SlowDownMovementEffectData : EffectData
    {
        public float percentageToReduceSpeed, durationOfEffect;
    }
}