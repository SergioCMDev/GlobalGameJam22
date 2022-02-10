using Application_.Events;
using Presentation.Building;
using UnityEngine;

namespace Presentation.Events
{
    [CreateAssetMenu(fileName = "BuildingHasBeenSetEvent", menuName = "Events/Grid/BuildingHasBeenSetEvent")]
    public class BuildingHasBeenSetEvent : GameEventScriptable
    {
        public GameObject Building;
        public MilitaryBuilding BuildingComponent;
        public Vector3Int Position;
    }
}