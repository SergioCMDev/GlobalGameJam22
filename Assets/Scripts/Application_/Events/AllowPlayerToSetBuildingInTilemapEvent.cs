using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayerSetBuildingInTilemapEvent",
        menuName = "Events/Building/PlayerSetBuildingInTilemapEvent")]
    public class AllowPlayerToSetBuildingInTilemapEvent : GameEventScriptable
    {
        public GameObject Prefab;
        public BuildingType BuildingType;
    }
}