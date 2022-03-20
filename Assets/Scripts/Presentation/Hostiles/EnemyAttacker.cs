using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Hostiles
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeField] private float attackSpeed = 1;
        [SerializeField] private float damage;
        private float _currentTime;

        public void Attack(IReceiveDamage receiveDamage)
        {
            _currentTime -= attackSpeed;
            receiveDamage.ReceiveDamage(damage);
        }


        public bool CanAttack()
        {
            _currentTime += Time.deltaTime;
            return _currentTime > attackSpeed;
        }
    }
}