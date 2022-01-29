using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayerSetBuildingInTilemapEvent", menuName = "Events/Building/PlayerSetBuildingInTilemapEvent")]
    public class PlayerSetBuildingInTilemapEvent : GameEventScriptable
    {
        public GameObject Prefab;
        public SelectedTileData SelectedTile;
    }
}