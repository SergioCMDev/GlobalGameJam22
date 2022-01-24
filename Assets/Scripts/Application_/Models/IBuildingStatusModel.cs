using System.Collections.Generic;

namespace Application_.Models
{
    public interface IBuildingStatusModel
    {
        public List<BuildStatus> BuildStatusList { get; set; }
        void AddBuilding(BuildStatus buildStatus);
    }
}