using Presentation.Hostiles;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "ImpactBehaviour", menuName = "Turrets/BehaviourAttacks/ImpactBehaviour")]
    public class ImpactDamageEffect : AttackBehaviourSO
    {
        private float _damageAmount;

        public override void Init(EffectData effectData)
        {
            _damageAmount = effectData.damage;
            MoneyToReceiveAfterHitEnemy = effectData.moneyToReceiveAfterHitEnemy;
        }

        public override void DoAttack(Enemy receiveDamage)
        {
            receiveDamage.ReceiveDamage(_damageAmount);
        }
    }
}