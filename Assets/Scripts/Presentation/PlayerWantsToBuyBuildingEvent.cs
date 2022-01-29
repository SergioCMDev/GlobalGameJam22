using Application_.Events;
using Presentation.Structs;
using UnityEngine;

namespace Presentation
{
    [CreateAssetMenu(fileName = "PlayerWantsToBuyBuildingEvent",
        menuName = "Events/Building/PlayerWantsToBuyBuildingEvent")]
    public class PlayerWantsToBuyBuildingEvent : GameEventScriptable
    {
        public BuildingTypeTuple Tuple;
    }
}