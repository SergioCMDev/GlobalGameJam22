using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "SlowDownMovementEffectData", menuName = "Turrets/AttackBehaviourData/SlowDownMovementEffectData")]
    public class SlowDownMovementEffectData : EffectData
    {
        public float percentageToReduceSpeed;
    }
}