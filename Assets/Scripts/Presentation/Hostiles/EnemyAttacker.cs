using App;
using Presentation.Interfaces;
using Presentation.Managers;
using UnityEngine;

namespace Presentation.Hostiles
{
    public class EnemyAttacker : MonoBehaviour
    {
        private float attackSpeed = 1;
        private float damage;
        private float _currentTime;

        public void Attack(IReceiveDamage receiveDamage)
        {
            _currentTime -= attackSpeed;
            receiveDamage.ReceiveDamage(damage);
        }

        public void Init(EnemyInfo enemyInfo)
        {
            attackSpeed = enemyInfo.attackSpeed;
            damage = enemyInfo.damage;
        }

        public bool CanAttack()
        {
            _currentTime += Time.deltaTime;
            return _currentTime > attackSpeed;
        }
    }
}