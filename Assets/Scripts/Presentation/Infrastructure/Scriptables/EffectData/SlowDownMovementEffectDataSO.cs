using UnityEngine;

namespace Presentation.Infrastructure.Scriptables.EffectData
{
    [CreateAssetMenu(fileName = "SlowDownMovementEffectData", menuName = "Turrets/AttackBehaviourData/SlowDownMovementEffectData")]
    public class SlowDownMovementEffectDataSO : EffectDataSO
    {
        public float percentageToReduceSpeed;
    }
}