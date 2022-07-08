using Presentation.Hostiles;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "TeslaBehaviourAttack", menuName = "Turrets/BehaviourAttacks/Tesla")]
    public class SlowDownMovementDuringTimeEffectSO : AttackBehaviourSO
    {
        private float _damageAmount;
        private float _percentageToReduce, _durationOfEffect;
        private IReceiveDamage _damageReceiver;
        private IStatusApplier _statusApplier;

        public override void Init(EffectData effectData)
        {
            var teslaMilitaryData = (SlowDownMovementDuringTimeEffectData)effectData;
            _percentageToReduce = teslaMilitaryData.percentageToReduceSpeed;
            _durationOfEffect = teslaMilitaryData.durationOfEffect;
            MoneyToReceiveAfterHitEnemy = effectData.moneyToReceiveAfterHitEnemy;
        }
        
        public override void DoAttack(Enemy receiveDamage)
        {
            receiveDamage.ReduceSpeed(_percentageToReduce, _durationOfEffect);
        }
    }
}