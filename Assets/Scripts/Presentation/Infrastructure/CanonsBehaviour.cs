using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure
{
    [CreateAssetMenu(fileName = "CanonsBehaviourAttack", menuName = "Turrets/BehaviourAttacks/Canons")]
    public class CanonsBehaviour : AttackBehaviour
    {
        private float _damageAmount;

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