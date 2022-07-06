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

        void Start()
        {
            _resourcesModel = ServiceLocator.Instance.GetModel<IResourcesModel>();
        }

        //TODO FINISH
        public bool PlayerHasEnoughResources(float goldToSpend)
        {
            return goldToSpend <= _resourcesModel.Gold ;
        }

        public void PlayerGetResource(PlayerGetResourceEvent playerGetResourceEvent)
        {
            AddResources(playerGetResourceEvent.Type, playerGetResourceEvent.Quantity);
        }

        public void RemoveResourcesOfPlayer(ResourcesTuple resourcesNeededForCurrentBuy)
        {
            var previousGoldQuantity = _resourcesModel.Gold;

            _resourcesModel.Gold -= resourcesNeededForCurrentBuy.Gold;
            _resourcesModel.Metal -= resourcesNeededForCurrentBuy.Metal;
            ThrowEventToUpdateUI(previousGoldQuantity, _resourcesModel.Gold);
        }

        private void AddResources(RetrievableResourceType type, int quantity)
        {
            Debug.Log($"Got resource {type} Q {quantity}");
            
            var previousGoldQuantity = _resourcesModel.Gold;
            _resourcesModel.AddResources(type, quantity);
            ThrowEventToUpdateUI(previousGoldQuantity, _resourcesModel.Gold);
        }

        private void ThrowEventToUpdateUI(int previousGoldQuantity, int currentGoldQuantity)
        {
            _updateUIResourcesEvent.previousQuantity = previousGoldQuantity;
            _updateUIResourcesEvent.currentQuantity = currentGoldQuantity;
            _updateUIResourcesEvent.Fire();
        }

        public float GetGold()
        {
            return _resourcesModel.Gold;
        }
    }
}