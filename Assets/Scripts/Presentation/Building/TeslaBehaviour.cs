using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Building
{
    [CreateAssetMenu(fileName = "TeslaBehaviourAttack", menuName = "Turrets/BehaviourAttacks/Tesla")]
    public class TeslaBehaviour : AttackBehaviour
    {
        private float _damageAmount;
        private float _percentageToReduce, _durationOfEffect;
        private IReceiveDamage _damageReceiver;
        private IStatusApplier _statusApplier;

        public override void Init(GameObject objectToAttack, MilitaryBuildingData militaryBuildingData)
        {
            _damageReceiver = objectToAttack.GetComponent<IReceiveDamage>();
            _statusApplier = objectToAttack.GetComponent<IStatusApplier>();
            
            var teslaMilitaryData = (TeslaMilitaryBuildingData)militaryBuildingData;
            _percentageToReduce = teslaMilitaryData.percentageToReduceSpeed;
            _durationOfEffect = teslaMilitaryData.durationOfEffect;
        }

        public override void DoAttack()
        {
            _damageReceiver.ReceiveDamage(_damageAmount);
            _statusApplier.ReduceSpeed(_percentageToReduce, _durationOfEffect);
        }
    }
}