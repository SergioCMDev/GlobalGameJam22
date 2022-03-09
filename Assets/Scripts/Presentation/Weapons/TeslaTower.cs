using System;
using System.Linq;
using Presentation.Building;
using UnityEngine;

namespace Presentation.Weapons
{
    public class TeslaTower : MilitaryBuildingFacade
    {
        protected override void ThrowParticlesWhenAttacks()
        {
            throw new NotImplementedException();
        }

        protected override bool CanReach()
        {
            return tilesToAttack.Any(tile => tile.IsOccupied && tile.Occupier != gameObject);
        }
    }
}