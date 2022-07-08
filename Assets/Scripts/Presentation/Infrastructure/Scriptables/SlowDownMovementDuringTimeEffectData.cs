using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "SlowDownMovementDuringTimeEffectData", menuName = "Turrets/AttackBehaviourData/SlowDownMovementDuringTimeEffectData")]
    public class SlowDownMovementDuringTimeEffectData : EffectData
    {
        public float percentageToReduceSpeed,durationOfEffect;
    }
}