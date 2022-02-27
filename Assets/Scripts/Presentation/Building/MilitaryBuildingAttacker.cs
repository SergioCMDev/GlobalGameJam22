using System;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Building
{
    public class MilitaryBuildingAttacker : MonoBehaviour, IAttack
    {

        //TODO TO BASIC TURRET?
        [SerializeField] private AttackRangeType _attackAreaType;
        [SerializeField] private DamageType _damageType;
        private float _cadence, _damage;
        private GameObject enemyGameObject;
        //TODO TO BASIC TURRET?

        private float Cadence
        {
            get => _cadence;
            set => _cadence = value;
        }

        private float Damage
        {
            get => _damage;
            set => _damage = value;
        }


        public AttackRangeType AttackAreaType => _attackAreaType;


        private float _lastTimeAttacked;
        private bool _enemyIsSet;


        public event Action OnBuildingAttacks;

  
        public void Attack(IReceiveDamage objectToAttack)
        {
            _lastTimeAttacked = Time.deltaTime;

            OnBuildingAttacks.Invoke();

            // objectToAttack.ReceiveDamage(Damage, _damageType);
        }


        //TODO REFRACTOR
        public bool CanAttack()
        {
            _lastTimeAttacked += Time.deltaTime;
            return _lastTimeAttacked > Cadence;
        }

        private void OnDrawGizmos()
        {
            if (!_enemyIsSet) return;
            Gizmos.color = Color.magenta;
            var directionToEnemy = enemyGameObject.transform.position - transform.position;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy) * 1);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy));
        }


        public void Init(GameObject enemy, float cadence , float damage)
        {
            _enemyIsSet = true;
            _cadence = cadence;
            _damage = damage;
            enemyGameObject = enemy;
        }
    }
}