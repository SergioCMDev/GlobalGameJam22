using Presentation.Structs;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    public abstract class EffectData : ScriptableObject
    {
        public DamageType damageType;
        public float cadence, damage;
        public int moneyToReceiveAfterHitEnemy;
    }
}