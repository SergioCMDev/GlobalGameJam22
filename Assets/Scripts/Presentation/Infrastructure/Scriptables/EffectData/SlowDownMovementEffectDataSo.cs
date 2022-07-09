using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "SlowDownMovementEffectData", menuName = "Turrets/AttackBehaviourData/SlowDownMovementEffectData")]
    public class SlowDownMovementEffectDataSO : EffectDataSO
    {
        public float percentageToReduceSpeed;
    }
}