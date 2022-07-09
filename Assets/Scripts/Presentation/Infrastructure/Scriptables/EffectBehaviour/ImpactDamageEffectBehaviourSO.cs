using Presentation.Hostiles;
using Presentation.Infrastructure.Scriptables.EffectData;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables.EffectBehaviour
{
    [CreateAssetMenu(fileName = "ImpactBehaviour1", menuName = "Turrets/AttacksBehaviour/ImpactBehaviour1")]
    public class ImpactDamageEffectBehaviourSO : AttackBehaviourSO
    {
        private float _damageAmount;

        public override void Init(EffectDataSO effectDataSo)
        {
            _damageAmount = effectDataSo.damage;
            MoneyToReceiveAfterHitEnemy = effectDataSo.moneyToReceiveAfterHitEnemy;
        }

        public override void DoAttack(Enemy receiveDamage)
        {
            receiveDamage.ReceiveDamage(_damageAmount);
        }
    }
}