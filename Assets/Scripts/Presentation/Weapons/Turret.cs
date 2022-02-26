using Presentation.Building;
using UnityEngine;
using Utils;

namespace Presentation.Weapons
{
    public class Turret : MilitaryBuildingFacade
    {
        
        protected override void ThrowParticlesWhenAttacks()
        {
            // throw new NotImplementedException();
        }


        protected override bool CanReach(GameObject objectToAttack)
        {
            return VectorUtils.VectorIsNearVector(gameObject.transform.position, objectToAttack.transform.position,
                DistanceToAttack);
        }
    }
}