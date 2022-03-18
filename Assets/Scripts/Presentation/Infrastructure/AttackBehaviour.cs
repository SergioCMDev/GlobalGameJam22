using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public abstract class AttackBehaviour : ScriptableObject
    {
        public abstract void Init(MilitaryBuildingData militaryBuildingData);

        public abstract void DoAttack(GameObject receiveDamage);
    }
}