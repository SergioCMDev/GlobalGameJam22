using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Building
{
    [CreateAssetMenu(fileName = "TeslaBehaviourAttack", menuName = "Turrets/BehaviourAttacks/Tesla")]
    public class TeslaBehaviour : AttackBehaviour
    {
        private float _damageAmount;
        private IReceiveDamage _damageReceiver;
        private IStatusApplier _statusApplier;

        public override void Init(GameObject objectToAttack, AttackBehaviourData attackBehaviourData)
        {
            _damageReceiver = objectToAttack.GetComponent<IReceiveDamage>();
            _statusApplier = objectToAttack.GetComponent<IStatusApplier>();
        }

        public override void DoAttack()
        {
            _damageReceiver.ReceiveDamage(_damageAmount);
            _statusApplier.ReduceSpeed(_damageAmount, _damageAmount);
        }
    }
}