using Presentation.Interfaces;
using UnityEngine;

namespace Presentation.Hostiles
{
    public class EnemyAttacker : MonoBehaviour
    {
        // private GameObject target;
        [SerializeField] private float attackSpeed = 1;
        [SerializeField] private float damage = 1f; //mover a enemy
        private float currentTime = 0; //mover a enemy

        public void Attack(IReceiveDamage receiveDamage)
        {
            currentTime -= attackSpeed;
            receiveDamage.ReceiveDamage(damage);
            // cityBuilding.ReceiveDamage(damage, _cityBuilds.Count);
        }


        public bool CanAttack()
        {
            currentTime += Time.deltaTime;
            return currentTime > attackSpeed;
        }
    }
}