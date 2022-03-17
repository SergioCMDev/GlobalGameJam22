using System;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class MilitaryBuildingAttacker : MonoBehaviour, IAttack
    {
        [SerializeField] private AttackBehaviour _attackBehaviour;
        [SerializeField] private MilitaryBuildingData _militaryBuildingData;
        private float _cadence;
        private bool _enemyIsSet;
        private float _lastTimeAttacked;

        private GameObject _enemyGameObject;

        public event Action OnBuildingAttacks;
        
        public void Attack(GameObject objectToAttack)
        {
            _lastTimeAttacked = Time.deltaTime;

            OnBuildingAttacks.Invoke();
            _attackBehaviour.DoAttack();
        }

        public bool CanAttack()
        {
            _lastTimeAttacked += Time.deltaTime;
            return _lastTimeAttacked > _cadence;
        }

        private void OnDrawGizmos()
        {
            if (!_enemyIsSet) return;
            Gizmos.color = Color.magenta;
            var directionToEnemy = _enemyGameObject.transform.position - transform.position;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy) * 1);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy));
        }

        public void Init(GameObject enemy)
        {
            _enemyIsSet = true;
            _cadence = _militaryBuildingData.cadence;
            _enemyGameObject = enemy;
            _attackBehaviour.Init(_enemyGameObject, _militaryBuildingData);
        }
    }
}