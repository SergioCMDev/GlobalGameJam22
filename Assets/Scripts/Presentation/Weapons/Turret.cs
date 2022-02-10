using System;
using Presentation.Building;

namespace Presentation.Weapons
{
    public class Turret : MilitaryBuilding
    {
        protected override void ThrowParticlesWhenAttacks()
        {
            // throw new NotImplementedException();
        }


        protected override bool CanReach()
        {
            throw new NotImplementedException();
        }
    }
}