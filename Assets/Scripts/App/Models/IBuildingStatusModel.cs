using System.Collections.Generic;

namespace App.Models
{
    public interface IBuildingStatusModel
    {
        public List<BuildStatus> BuildStatusList { get; set; }
        void AddBuilding(BuildStatus buildStatus);
    }
}