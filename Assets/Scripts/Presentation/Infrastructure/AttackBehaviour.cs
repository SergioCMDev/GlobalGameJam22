using Presentation.Hostiles;
using Presentation.Infrastructure.Scriptables;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public abstract class AttackBehaviour : ScriptableObject
    {
        public abstract void Init(MilitaryBuildingData militaryBuildingData);

        public abstract void DoAttack(Enemy receiveDamage);
    }
}