using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayerSetBuildingInTilemapEvent",
        menuName = "Events/Building/PlayerSetBuildingInTilemapEvent")]
    public class PlayerSetBuildingInTilemapEvent : GameEventScriptable
    {
        public GameObject Prefab;
        public Vector3 WolrdPosition;
        public Vector3Int GridPosition;
    }
}