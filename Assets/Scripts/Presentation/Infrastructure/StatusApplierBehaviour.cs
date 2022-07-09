using Presentation.Infrastructure.Scriptables;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public abstract class StatusApplierBehaviour : ScriptableObject
    {
        public abstract void Init(EffectDataSO effectDataSo);

        public abstract void ApplyStatus(GameObject receiveDamage);
    }
}