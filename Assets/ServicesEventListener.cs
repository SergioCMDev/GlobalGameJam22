using System;
using App.Events;
using Services.MilitaryBuilding;
using Services.ResourcesManager;
using UnityEngine;
using Utils;

public class ServicesEventListener : MonoBehaviour
{
    private MilitaryBuildingService _militaryBuildingService;
    private ResourcesManagerService _resourcesManagerService;
    
    private void Start()
    {
        _militaryBuildingService = ServiceLocator.Instance.GetService<MilitaryBuildingService>();
        _resourcesManagerService = ServiceLocator.Instance.GetService<ResourcesManagerService>();
    }

    public void SaveBuilding(SaveBuildingEvent saveBuildingEvent)
    {
        _militaryBuildingService.SaveBuilding(saveBuildingEvent);
    }
    
    public void PlayerGetResource(PlayerGetResourceEvent playerGetResourceEvent)
    {
        _resourcesManagerService.PlayerGetResource(playerGetResourceEvent);
    }
    
    public void DeactivateBuilding(DeactivateMilitaryBuildingsEvent _)
    {
        _militaryBuildingService.DeactivateMilitaryBuildings();
    }
    
    public void ActivateBuilding(ActivateMilitaryBuildingsEvent _)
    {
        _militaryBuildingService.ActivateMilitaryBuildings();
    }
}