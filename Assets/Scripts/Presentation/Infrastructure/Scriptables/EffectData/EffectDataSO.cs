using UnityEngine;

namespace Presentation.Infrastructure.Scriptables.EffectData
{
    public abstract class EffectDataSO : ScriptableObject
    {
        public float damage;
        public int moneyToReceiveAfterHitEnemy;
    }
}