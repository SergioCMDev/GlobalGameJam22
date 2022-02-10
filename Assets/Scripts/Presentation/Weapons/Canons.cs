using System;
using Presentation.Building;

namespace Presentation.Weapons
{
    public class Canons : MilitaryBuilding
    {
        protected override void ThrowParticlesWhenAttacks()
        {
            throw new NotImplementedException();
        }


        protected override bool CanReach()
        {
            throw new NotImplementedException();
        }
    }
}