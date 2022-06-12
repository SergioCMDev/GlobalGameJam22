using Presentation.Hostiles;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
{
    [CreateAssetMenu(fileName = "CanonsBehaviourAttack", menuName = "Turrets/BehaviourAttacks/Canons")]
    public class CanonsBehaviour : AttackBehaviour
    {
        private float _damageAmount;

        public override void Init(MilitaryBuildingData militaryBuildingData)
        {
            _damageAmount = militaryBuildingData.damage;
        }

        public override void DoAttack(Enemy receiveDamage)
        {
            receiveDamage.ReceiveDamage(_damageAmount);
        }
    }
}