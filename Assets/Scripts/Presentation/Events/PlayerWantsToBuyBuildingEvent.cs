using App;
using App.Events;
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