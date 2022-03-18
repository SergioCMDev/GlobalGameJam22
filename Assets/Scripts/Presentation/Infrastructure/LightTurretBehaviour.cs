using Presentation.Hostiles;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure
{
    [CreateAssetMenu(fileName = "LightTurretBehaviourAttack", menuName = "Turrets/BehaviourAttacks/LightTurret")]
    public class LightTurretBehaviour : AttackBehaviour
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