using Presentation.Hostiles;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "SlowDownMovementEffect", menuName = "Turrets/BehaviourAttacks/SlowDownMovementEffect")]
    public class SlowDownMovementEffectSO : AttackBehaviourSO
    {
        private float _damageAmount;
        private float _percentageToReduce;
        private IReceiveDamage _damageReceiver;
        private IStatusApplier _statusApplier;

        public override void Init(EffectData effectData)
        {
            var teslaMilitaryData = (SlowDownMovementEffectData)effectData;
            _percentageToReduce = teslaMilitaryData.percentageToReduceSpeed;
            MoneyToReceiveAfterHitEnemy = effectData.moneyToReceiveAfterHitEnemy;
        }
        
        public override void DoAttack(Enemy receiveDamage)
        {
            receiveDamage.ReduceSpeed(_percentageToReduce);
        }
    }
}