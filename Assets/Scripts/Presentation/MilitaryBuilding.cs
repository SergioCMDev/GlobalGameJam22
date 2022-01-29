using UnityEngine;

namespace Presentation
{
    public class MilitaryBuilding : Building, IAttack{
        public override void ReceiveDamage(GameObject itemWhichHit, float receivedDamage)
        {
            throw new System.NotImplementedException();
        }

        public override void ReceiveDamage(float receivedDamage)
        {
            throw new System.NotImplementedException();
        }

        public override void AddLife(float lifeToAdd)
        {
            throw new System.NotImplementedException();
        }

        public void Attack(IReceiveDamage objectToAttack)
        {
            throw new System.NotImplementedException();
        }
        
        private void Update()
        {
            // Attack();
        }

    }
}