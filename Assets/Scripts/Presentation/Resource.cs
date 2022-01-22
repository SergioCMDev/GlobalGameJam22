using Application_;
using Application_.Events;
using UnityEngine;
using Utils;

namespace Presentation
{
    public abstract class Resource : MonoBehaviour, IReceiveDamage
    {
        [SerializeField] protected RetrievableResourceType _resourceType;
        [SerializeField] protected PlayerGetResourceEvent playerGetResourceEvent;

        public abstract void ReceiveDamage(GameObject itemWhichHit, float receivedDamage);

        public abstract void ReceiveDamage(float receivedDamage);

        private int GetRandomQuantity()
        {
            return Utilities.GetRandom(0, 100);
        }

        protected void RetrieveMaterial()
        {
            var quantity = GetRandomQuantity();
            playerGetResourceEvent.Quantity = quantity;
            playerGetResourceEvent.Type = _resourceType;
            playerGetResourceEvent.Fire();
        }
    }
}