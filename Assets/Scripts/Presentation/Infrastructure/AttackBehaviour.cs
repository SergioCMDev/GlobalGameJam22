using Presentation.Hostiles;
using Presentation.Infrastructure.Scriptables;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public abstract class AttackBehaviour : ScriptableObject
    {
        protected int MoneyToReceiveAfterHitEnemy;

        public abstract void Init(MilitaryBuildingData militaryBuildingData);

        public abstract void DoAttack(Enemy receiveDamage);

        public int GetMoneyOfAttack()
        {
            return MoneyToReceiveAfterHitEnemy;
        }
    }
}