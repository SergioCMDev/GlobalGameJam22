using Presentation.Hostiles;
using UnityEngine;

namespace Presentation.Infrastructure.Scriptables
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