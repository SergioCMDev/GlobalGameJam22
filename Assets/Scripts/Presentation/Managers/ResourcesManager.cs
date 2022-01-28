using Application_;
using Application_.Events;
using Application_.Models;
using Presentation.Structs;
using UnityEngine;
using Utils;

namespace Presentation.Managers
{
    public class ResourcesManager : MonoBehaviour
    {
        private IResourcesModel _resourcesModel;

        void Start()
        {
            _resourcesModel = ServiceLocator.Instance.GetModel<IResourcesModel>();
        }

        public bool PlayerHasEnoughResources(float goldToSpend, float metalToSpend)
        {
            // return goldToSpend <= _resourcesModel.Gold && metalToSpend <= _resourcesModel.Metal;
            return true;
        }

//From Event
        public void PlayerGetResource(PlayerGetResourceEvent playerGetResourceEvent)
        {
            Debug.Log($"Got resource {playerGetResourceEvent.Type} Q {playerGetResourceEvent.Quantity}");
            _resourcesModel.AddResources(playerGetResourceEvent.Type, playerGetResourceEvent.Quantity);
            //IF player Gets resource check if any building can be updated based on a scriptable object with level-resource quantities and type of upgrade
        }

        public void RemoveResourcesOfPlayer(ResourcesTuple resourcesNeededForCurrentBuy)
        {
            _resourcesModel.Gold -= resourcesNeededForCurrentBuy.Gold;
            _resourcesModel.Metal -= resourcesNeededForCurrentBuy.Metal;
        }
    }
}