using App;
using App.Events;
using App.Models;
using Presentation.Structs;
using UnityEngine;
using Utils;

namespace Presentation.Managers
{
    public class ResourcesManager : MonoBehaviour
    {
        [SerializeField] private UpdateUIResourcesEvent _updateUIResourcesEvent;
        private IResourcesModel _resourcesModel;
        public IResourcesModel ResourcesModel => _resourcesModel;

        void Start()
        {
            _resourcesModel = ServiceLocator.Instance.GetModel<IResourcesModel>();
        }

        //TODO FINISH
        public bool PlayerHasEnoughResources(float goldToSpend, float metalToSpend)
        {
            // return goldToSpend <= _resourcesModel.Gold && metalToSpend <= _resourcesModel.Metal;
            return goldToSpend <= _resourcesModel.Gold ;
            // return true;
        }

        public void PlayerGetResource(PlayerGetResourceEvent playerGetResourceEvent)
        {
            AddResources(playerGetResourceEvent.Type, playerGetResourceEvent.Quantity);
        }

        public void RemoveResourcesOfPlayer(ResourcesTuple resourcesNeededForCurrentBuy)
        {
            ResourcesModel.Gold -= resourcesNeededForCurrentBuy.Gold;
            ResourcesModel.Metal -= resourcesNeededForCurrentBuy.Metal;
            _updateUIResourcesEvent.Fire();
        }

        public void AddResources(RetrievableResourceType type, float quantity)
        {
            Debug.Log($"Got resource {type} Q {quantity}");
            ResourcesModel.AddResources(type, quantity);
            _updateUIResourcesEvent.Fire();
        }
    }
}