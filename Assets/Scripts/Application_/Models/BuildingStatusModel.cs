using System.Collections.Generic;

namespace Application_.Models
{
    public class BuildingStatusModel : IBuildingStatusModel{
        public List<BuildStatus> BuildStatusList  { get; set; }
    }
}