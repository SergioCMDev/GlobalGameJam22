using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure
{
    [CreateAssetMenu(fileName = "TeslaBehaviourAttack", menuName = "Turrets/BehaviourAttacks/Tesla")]
    public class TeslaBehaviour : AttackBehaviour
    {
        private float _damageAmount;
        private float _percentageToReduce, _durationOfEffect;
        private IReceiveDamage _damageReceiver;
        private IStatusApplier _statusApplier;

        public override void Init(MilitaryBuildingData militaryBuildingData)
        {
            var teslaMilitaryData = (TeslaMilitaryBuildingData)militaryBuildingData;
            _percentageToReduce = teslaMilitaryData.percentageToReduceSpeed;
            _durationOfEffect = teslaMilitaryData.durationOfEffect;
            _damageAmount = teslaMilitaryData.damage;
        }

        private void ApplySpeedReduction(GameObject receiveDamage)
        {
            receiveDamage.GetComponent<IStatusApplier>().ReduceSpeed(_percentageToReduce, _durationOfEffect);
        }

        public override void DoAttack(GameObject receiveDamage)
        {
            receiveDamage.GetComponent<IReceiveDamage>().ReceiveDamage(_damageAmount);
            ApplySpeedReduction(receiveDamage);
        }
    }
}