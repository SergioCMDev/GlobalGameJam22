using System;
using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Building
{
    public class MilitaryBuildingAttacker : MonoBehaviour, IAttack
    {
        //TODO TO BASIC TURRET?
        [SerializeField] private Vector3Int _attackArea;

        //TODO TO BASIC TURRET?
        [SerializeField] private AttackRangeType _attackAreaType;
        [SerializeField] private int _attackRingRange = 1;
        [SerializeField] private DamageType _damageType;
        private float _cadence, _damage, _distanceToAttack;
        private GameObject enemyGameObject;

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

        public Vector3Int AttackArea => _attackArea;

        public AttackRangeType AttackAreaType => _attackAreaType;

        public int AttackRingRange => _attackRingRange;

        private float _lastTimeAttacked;
        private bool _enemyIsSet;


        public event Action OnBuildingAttacks;

        private void Awake()
        {
            // _attackArea = new Vector3Int(3 * _attackRingRange, 3 * _attackRingRange, 1);
        }

        public void Attack(IReceiveDamage objectToAttack)
        {
            _lastTimeAttacked = Time.deltaTime;

            OnBuildingAttacks.Invoke();

            objectToAttack.ReceiveDamage(Damage, _damageType);
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
            Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy) * _distanceToAttack);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(directionToEnemy));
        }

        public void Init(GameObject enemy, float cadence, float distanceToAttack, float damage)
        {
            _enemyIsSet = true;
            _cadence = cadence;
            _damage = damage;
            _distanceToAttack = distanceToAttack;
            enemyGameObject = enemy;
        }
    }
}