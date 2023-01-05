using System.Collections.Generic;
using App.Buildings;

namespace App.Models
{
    public interface IBuildingStatusModel
    {
        public List<BuildStatus> BuildStatusList { get; set; }
        void AddBuilding(BuildStatus buildStatus);
    }
}