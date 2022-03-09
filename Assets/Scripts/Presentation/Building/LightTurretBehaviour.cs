using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Building
{
    [CreateAssetMenu(fileName = "LightTurretBehaviourAttack", menuName = "Turrets/BehaviourAttacks/LightTurret")]
    public class LightTurretBehaviour : AttackBehaviour
    {
        private float _damageAmount;
        private IReceiveDamage _damageReceiver;

        public override void Init(GameObject objectToAttack, AttackBehaviourData attackBehaviourData)
        {
            _damageReceiver = objectToAttack.GetComponent<IReceiveDamage>();
        }

        public override void DoAttack()
        {
            _damageReceiver.ReceiveDamage(_damageAmount);
        }
    }
}