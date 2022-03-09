using UnityEngine;

namespace Presentation.Building
{
    public abstract class AttackBehaviour : ScriptableObject
    {
        public abstract void Init(GameObject objectToAttack, MilitaryBuildingData militaryBuildingData);
        public abstract void DoAttack();
    }
}