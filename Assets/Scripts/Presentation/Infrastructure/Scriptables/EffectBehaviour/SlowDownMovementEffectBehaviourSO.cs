using Presentation.Hostiles;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "SlowDownMovementEffect", menuName = "Turrets/AttacksBehaviour/SlowDownMovementEffect")]
    public class SlowDownMovementEffectBehaviourSO : AttackBehaviourSO
    {
        private float _damageAmount;
        private float _percentageToReduce;
        private IReceiveDamage _damageReceiver;
        private IStatusApplier _statusApplier;

        public override void Init(EffectDataSO effectDataSo)
        {
            var convertedEffectData = (SlowDownMovementEffectDataSO)effectDataSo;
            _percentageToReduce = convertedEffectData.percentageToReduceSpeed;
            MoneyToReceiveAfterHitEnemy = effectDataSo.moneyToReceiveAfterHitEnemy;
        }
        
        public override void DoAttack(Enemy receiveDamage)
        {
            receiveDamage.ReduceSpeed(_percentageToReduce);
        }
    }
}