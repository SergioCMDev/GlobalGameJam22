using System.Collections.Generic;
using App.Buildings;

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
                MilitaryBuildingType = buildStatus.MilitaryBuildingType,
                MaxLife = buildStatus.MaxLife,
                level = buildStatus.level
            };
            item.CurrentLife = item.MaxLife;
            BuildStatusList.Add(item);
        }
    }
}