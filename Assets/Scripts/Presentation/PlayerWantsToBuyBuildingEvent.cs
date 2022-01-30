using Application_;
using Application_.Events;
using UnityEngine;

namespace Presentation
{
    [CreateAssetMenu(fileName = "PlayerWantsToBuyBuildingEvent",
        menuName = "Events/Building/PlayerWantsToBuyBuildingEvent")]
    public class PlayerWantsToBuyBuildingEvent : GameEventScriptable
    {
        public BuildingType BuildingType;
    }
}