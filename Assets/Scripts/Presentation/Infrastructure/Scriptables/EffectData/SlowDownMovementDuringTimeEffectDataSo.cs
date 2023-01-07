using UnityEngine;

namespace Presentation.Infrastructure.Scriptables.EffectData
{
    [CreateAssetMenu(fileName = "SlowDownMovementDuringTimeEffectData", menuName = "Turrets/AttackBehaviourData/SlowDownMovementDuringTimeEffectData")]
    public class SlowDownMovementDuringTimeEffectDataSo : EffectDataSO
    {
        public float percentageToReduceSpeed,durationOfEffect;
    }
}