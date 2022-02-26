using System;
using Presentation.Building;
using UnityEngine;

namespace Presentation.Weapons
{
    public class Canons : MilitaryBuilding
    {
        protected override void ThrowParticlesWhenAttacks()
        {
            throw new NotImplementedException();
        }


        protected override bool CanReach(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}