using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "PlayerSetBuildingInTilemapEvent",
        menuName = "Events/Building/PlayerSetBuildingInTilemapEvent")]
    public class AllowPlayerToSetBuildingInTilemapEvent : GameEventScriptable
    {
        public GameObject Prefab;
        public MiltaryBuildingType miltaryBuildingType;
    }
}