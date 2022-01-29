using Application_;

namespace Presentation
{
    public class TeslaTowerSelector : BuildingSelector
    {
        private void Awake()
        {
            BuildingType = BuildingType.Tesla;
        }
    }
}