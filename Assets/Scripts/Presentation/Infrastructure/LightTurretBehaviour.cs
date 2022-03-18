using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure
{
    [CreateAssetMenu(fileName = "LightTurretBehaviourAttack", menuName = "Turrets/BehaviourAttacks/LightTurret")]
    public class LightTurretBehaviour : AttackBehaviour
    {
        private float _damageAmount;
        private IReceiveDamage _damageReceiver;

        public override void Init(MilitaryBuildingData militaryBuildingData)
        {
            _damageAmount = militaryBuildingData.damage;
        }
        
        public override void DoAttack(GameObject receiveDamage)
        {
            receiveDamage.GetComponent<IReceiveDamage>().ReceiveDamage(_damageAmount);
        }
    }
}