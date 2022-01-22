using Application_;
using UnityEngine;

namespace Presentation
{
    public class Gold : Resource
    {
        private void Start()
        {
            _resourceType = RetrievableResourceType.Gold;
        }

        public override void ReceiveDamage(GameObject itemWhichHit, float receivedDamage)
        {
            RetrieveMaterial();
        }

        public override void ReceiveDamage(float receivedDamage)
        {
            throw new System.NotImplementedException();
        }
    }
}