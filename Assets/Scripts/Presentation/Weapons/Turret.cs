using System.Linq;
using Presentation.Building;
using UnityEngine;

namespace Presentation.Weapons
{
    public class Turret : MilitaryBuildingFacade
    {
        [SerializeField] private float _offsetForEachTile = 2;

        protected override void ThrowParticlesWhenAttacks()
        {
            // throw new NotImplementedException();
        }
        
        protected override bool CanReach(GameObject objectToAttack)
        {
            // return tilesToAttack.Any(tile => tile.IsOccupied && tile.Occupier == objectToAttack);
            return tilesToAttack.Any(tile => tile.IsOccupied && tile.Occupier != gameObject);
            
        }
    }
}