using Application_;
using Application_.Events;
using UnityEngine;

namespace Presentation.Events
{
    [CreateAssetMenu(fileName = "PlayerWantsToBuyBuildingEvent",
        menuName = "Events/Building/PlayerWantsToBuyBuildingEvent")]
    public class PlayerWantsToBuyBuildingEvent : GameEventScriptable
    {
        public BuildingType BuildingType;
    }
}