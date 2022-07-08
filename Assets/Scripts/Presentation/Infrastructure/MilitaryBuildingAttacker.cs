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
        [SerializeField] private List<AttackBehaviourSO> _attackBehaviour;
        [SerializeField] private EffectData effectData;
        private float _cadence;
        private float _lastTimeAttacked;
        private GameObject _enemyGameObject;
        private bool _hasAttackedBefore;

        public event Action OnBuildingAttacks;
        public event Action<int> OnAddMoneyToPlayer;

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
                   AddMoneyToPlayer(attackBehaviour.GetMoneyOfAttack());
                }
            }
            _hasAttackedBefore = true;

        }

        private void AddMoneyToPlayer(int quantity)
        {
            OnAddMoneyToPlayer?.Invoke(quantity);
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
            _cadence = effectData.cadence;
            foreach (var attackBehaviour in _attackBehaviour)
            {
                attackBehaviour.Init(effectData);
            }
        }
    }
}