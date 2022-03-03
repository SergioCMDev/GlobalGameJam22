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
            Debug.Log($"Got resource {playerGetResourceEvent.Type} Q {playerGetResourceEvent.Quantity}");
            ResourcesModel.AddResources(playerGetResourceEvent.Type, playerGetResourceEvent.Quantity);
            //IF player Gets resource check if any building can be updated based on a scriptable object with level-resource quantities and type of upgrade
            _updateUIResourcesEvent.Fire();

        }

        public void RemoveResourcesOfPlayer(ResourcesTuple resourcesNeededForCurrentBuy)
        {
            ResourcesModel.Gold -= resourcesNeededForCurrentBuy.Gold;
            ResourcesModel.Metal -= resourcesNeededForCurrentBuy.Metal;
            _updateUIResourcesEvent.Fire();
        }
    }
}