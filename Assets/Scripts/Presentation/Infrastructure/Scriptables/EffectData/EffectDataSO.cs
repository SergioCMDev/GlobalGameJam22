using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    public abstract class EffectDataSO : ScriptableObject
    {
        public float cadence, damage;
        public int moneyToReceiveAfterHitEnemy;
    }
}