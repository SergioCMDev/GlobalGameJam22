using Presentation.Hostiles;
using Presentation.Infrastructure.Scriptables.EffectData;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables.EffectBehaviour
{
    [CreateAssetMenu(fileName = "SlowDownMovementEffectDuringTimeBehaviourAttack",
        menuName = "Turrets/AttacksBehaviour/SlowDownMovementEffectDuringTime")]
    public class SlowDownMovementDuringTimeEffectBehaviourSO : AttackBehaviourSO
    {
        private float _damageAmount;
        private float _percentageToReduce, _durationOfEffect;
        private IReceiveDamage _damageReceiver;
        private IStatusApplier _statusApplier;

        public override void Init(EffectDataSO effectDataSo)
        {
            MoneyToReceiveAfterHitEnemy = effectDataSo.moneyToReceiveAfterHitEnemy;
            var convertedEffectData = effectDataSo as SlowDownMovementDuringTimeEffectDataSO;
            
            if (convertedEffectData == null) return;
            _percentageToReduce = convertedEffectData.percentageToReduceSpeed;
            _durationOfEffect = convertedEffectData.durationOfEffect;
        }

        public override void DoAttack(Enemy receiveDamage)
        {
            receiveDamage.ReduceSpeed(_percentageToReduce, _durationOfEffect);
        }
    }
}