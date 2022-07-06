using Presentation.Hostiles;
using Presentation.Infrastructure.Scriptables;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public abstract class AttackBehaviour : ScriptableObject
    {
        protected float MoneyToReceiveAfterHitEnemy;

        public abstract void Init(MilitaryBuildingData militaryBuildingData);

        public abstract void DoAttack(Enemy receiveDamage);

        public float GetMoneyOfAttack()
        {
            return MoneyToReceiveAfterHitEnemy;
        }
    }
}