using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    public abstract class EffectDataSO : ScriptableObject
    {
        public float damage;
        public int moneyToReceiveAfterHitEnemy;
    }
}