using Application_;
using Application_.Events;
using UnityEngine;
using Utils;

namespace Presentation
{
    public abstract class Resource : MonoBehaviour
    {
        [SerializeField] protected RetrievableResourceType _resourceType;
        [SerializeField] protected PlayerGetResourceEvent playerGetResourceEvent;
        
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