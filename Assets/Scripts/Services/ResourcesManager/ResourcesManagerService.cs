using System;
using System.Collections.Generic;
using App.Events;
using App.Models;
using App.Resources;
using UnityEngine;
using Utils;



namespace Services.ResourcesManager
{
    [Serializable]
    public struct InitialResourcesByLevel
    {
        public string levelName;
        public int initialGold;
    }
    [CreateAssetMenu(fileName = "ResourcesManagerService",
        menuName = "Loadable/Services/ResourcesManagerService")]
    public class ResourcesManagerService : LoadableComponent
    {    
        [SerializeField] private UpdateUIResourcesEvent updateUIResourcesEvent;
        [SerializeField] private List<InitialResourcesByLevel>  initialResources;
        private IResourcesModel _resourcesModel;
        private readonly int _initialResourceByDefault = 200;

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

        public void AddResources(RetrievableResourceType type, int quantity)
        {
            Debug.Log($"Got resource {type} Q {quantity}");
            
            var previousGoldQuantity = _resourcesModel.Gold;
            _resourcesModel.AddResources(type, quantity);
            ThrowEventToUpdateUI(previousGoldQuantity, _resourcesModel.Gold);
        }
        
        public void OverrideResources(RetrievableResourceType type, int quantity)
        {
            Debug.Log($"Override resource {type} Q {quantity}");
            
            _resourcesModel.OverrideResources(type, quantity);
        }

        private void ThrowEventToUpdateUI(int previousGoldQuantity, int currentGoldQuantity)
        {
            updateUIResourcesEvent.previousQuantity = previousGoldQuantity;
            updateUIResourcesEvent.currentQuantity = currentGoldQuantity;
            updateUIResourcesEvent.Fire();
        }

        public float GetGold()
        {
            return _resourcesModel.Gold;
        }
        
        public int GetInitialGoldByLevel(string levelId)
        {
            foreach (var initialResourcesByLevel in initialResources)
            {
                if (initialResourcesByLevel.levelName == levelId)
                {
                    return initialResourcesByLevel.initialGold;
                }
            }

            return _initialResourceByDefault;
        }

        public override void Execute()
        {
            _resourcesModel = ServiceLocator.Instance.GetModel<IResourcesModel>();
        }
    }
}