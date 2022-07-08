using Presentation.Structs;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "AttackBehaviourData", menuName = "Turrets/AttackBehaviourData/Turrets")]
    public class EffectData : ScriptableObject
    {
        public DamageType damageType;
        public float cadence, damage;
        public int moneyToReceiveAfterHitEnemy;
    }
}