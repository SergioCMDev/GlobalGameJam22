using UnityEngine;

namespace Presentation.Infrastructure
{
    public abstract class StatusApplierBehaviour : ScriptableObject
    {
        public abstract void Init(MilitaryBuildingData militaryBuildingData);

        public abstract void ApplyStatus(GameObject receiveDamage);
    }
}