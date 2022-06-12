using System;
using System.Collections.Generic;
using Presentation.Hostiles;
using Presentation.Infrastructure.Scriptables;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Infrastructure
{
    public class MilitaryBuildingAttacker : MonoBehaviour, IAttack
    {
        [SerializeField] private List<AttackBehaviour> _attackBehaviour;
        [SerializeField] private MilitaryBuildingData _militaryBuildingData;
        private float _cadence;
        private float _lastTimeAttacked;

        private GameObject _enemyGameObject;
        private bool _hasAttackedBefore;

        public event Action OnBuildingAttacks;

        public void Attack(List<Enemy> objectsToAttack)
        {
            Debug.Log("ATTACK");

            _lastTimeAttacked = Time.deltaTime;
            OnBuildingAttacks.Invoke();
            foreach (var receiveDamage in objectsToAttack)
            {
                foreach (var attackBehaviour in _attackBehaviour)
                {
                    attackBehaviour.DoAttack(receiveDamage);
                }
            }
            _hasAttackedBefore = true;

        }

        public bool CanAttack()
        {
            _lastTimeAttacked += Time.deltaTime;
            if (_hasAttackedBefore) return _lastTimeAttacked > _cadence;
            return true;
        }

        private void OnDrawGizmos()
        {
            // if (!_enemyIsSet) return;
            // Gizmos.color = Color.magenta;
            // var directionToEnemy = _enemyGameObject.transform.position - transform.position;
            // Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy) * 1);
            // Gizmos.color = Color.blue;
            // Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy));
        }

        public void Init()
        {
            _cadence = _militaryBuildingData.cadence;
            foreach (var attackBehaviour in _attackBehaviour)
            {
                attackBehaviour.Init(_militaryBuildingData);
            }
        }
    }
}