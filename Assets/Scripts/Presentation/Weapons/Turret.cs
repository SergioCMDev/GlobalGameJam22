using Presentation.Building;
using UnityEngine;
using Utils;

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
            // var distance = objectToAttack.transform.position - transform.position;
            // var magninute = distance.magnitude / 2;
            // return magninute < 2.5;

            var xDif = transform.position.x - objectToAttack.transform.position.x;
            var yDif = transform.position.y - objectToAttack.transform.position.y;
            var distance = yDif / 2 + xDif;
            var roundToInt = Mathf.RoundToInt(distance);
            Debug.Log($"DIstance {distance} xDif {xDif} ydif {yDif}");
            return roundToInt < AttackRingRange;
            // return Mathf.Pow(transform.position.x - transform.position.y, 2) +
            //        Mathf.Pow(objectToAttack.transform.position.x - objectToAttack.transform.position.y, 2) <=
            //        AttackRingRange * AttackRingRange;
            // return VectorUtils.VectorIsNearVector(transform.position, objectToAttack.transform.position,
            //     AttackRingRange * _offsetForEachTile);
        }
    }
}