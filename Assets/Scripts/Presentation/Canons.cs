using System;

namespace Presentation
{
    public class Canons : MilitaryBuilding
    {
        protected override void ThrowParticlesWhenAttacks()
        {
            throw new NotImplementedException();
        }

        protected override bool CanReach(IReceiveDamage objectToAttack)
        {
            throw new NotImplementedException();
        }

        protected override bool CanReach()
        {
            throw new NotImplementedException();
        }
    }
}