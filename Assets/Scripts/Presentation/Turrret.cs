using System;
using UnityEngine;

namespace Presentation
{
    public class Turrret : MilitaryBuilding
    {
        [SerializeField] private float _cadency;
        
        public override void ReceiveDamage(GameObject itemWhichHit, float receivedDamage)
        {
        }



        public override void ReceiveDamage(float receivedDamage)
        {
            Life -= receivedDamage;
            UpdateLifeSliderBar();
        }

        public override void AddLife(float lifeToAdd)
        {
            Life += lifeToAdd;
            UpdateLifeSliderBar();
        }

        public void Attack(IReceiveDamage objectToAttack)
        {
            objectToAttack.ReceiveDamage(1);
        }
    }
}
