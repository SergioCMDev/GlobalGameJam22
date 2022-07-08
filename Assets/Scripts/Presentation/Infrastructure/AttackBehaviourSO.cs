using Presentation.Hostiles;
using Presentation.Infrastructure.Scriptables;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public abstract class AttackBehaviourSO : ScriptableObject
    {
        protected int MoneyToReceiveAfterHitEnemy;

        public abstract void Init(EffectData effectData);

        public abstract void DoAttack(Enemy receiveDamage);

        public int GetMoneyOfAttack()
        {
            return MoneyToReceiveAfterHitEnemy;
        }
    }
}