using System;
using Presentation.Building;
using UnityEngine;

namespace Presentation.Hostiles
{
    public class EnemyAttacker : MonoBehaviour
    {
        private GameObject target;
        public float attackSpeed = 1;
        public float damage = 1f; //mover a enemy
        [SerializeField] private CityBuilding cityBuilding;
        private float currentTime = 0; //mover a enemy

        private void Update()
        {
            if (IsNearTarget() && CanAttack())
            {
                // cityBuilding.ReceiveDamage(damage, _cityBuilds.Count);
                currentTime -= attackSpeed;
            }
        }

        private bool IsNearTarget()
        {
            throw new NotImplementedException();
        }

        private bool CanAttack()
        {
            currentTime += Time.deltaTime;
            return currentTime > attackSpeed;
        }
    }
}