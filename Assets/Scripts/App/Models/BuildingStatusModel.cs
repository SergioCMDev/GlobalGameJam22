using System.Collections.Generic;

namespace App.Models
{
    public class BuildingStatusModel : IBuildingStatusModel{
        public List<BuildStatus> BuildStatusList  { get; set; }

        public BuildingStatusModel()
        {
            BuildStatusList = new List<BuildStatus>();
           
        }

        public void AddBuilding(BuildStatus buildStatus)
        {
            var item = new BuildStatus
            {
                buildingType = buildStatus.buildingType,
                MaxLife = buildStatus.MaxLife,
                level = buildStatus.level
            };
            item.CurrentLife = item.MaxLife;
            BuildStatusList.Add(item);
        }
    }
}