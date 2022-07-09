using Presentation.Hostiles;
using Presentation.Infrastructure.Scriptables.EffectData;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables.EffectBehaviour
{
    public abstract class AttackBehaviourSO : ScriptableObject
    {
        protected int MoneyToReceiveAfterHitEnemy;

        public abstract void Init(EffectDataSO effectDataSo);

        public abstract void DoAttack(Enemy receiveDamage);

        public int GetMoneyOfAttack()
        {
            return MoneyToReceiveAfterHitEnemy;
        }
    }
}