using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Building
{
    [CreateAssetMenu(fileName = "CanonsBehaviourAttack", menuName = "Turrets/BehaviourAttacks/Canons")]
    public class CanonsBehaviour : AttackBehaviour
    {
        private float _damageAmount;
        private IReceiveDamage _damageReceiver;

        public override void Init(GameObject objectToAttack, MilitaryBuildingData militaryBuildingData)
        {
            _damageReceiver = objectToAttack.GetComponent<IReceiveDamage>();
            _damageAmount = militaryBuildingData.damage;
        }

        public override void DoAttack()
        {
            _damageReceiver.ReceiveDamage(_damageAmount);
        }
    }
}