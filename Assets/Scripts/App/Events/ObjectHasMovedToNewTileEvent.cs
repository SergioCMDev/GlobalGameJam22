using App.Tuples;
using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "ObjectHasMovedToNewTileEvent",
        menuName = "Events/Movables/ObjectHasMovedToNewTileEvent")]
    public class ObjectHasMovedToNewTileEvent : GameEventScriptable
    {
        public GameObject Occupier;
        
        public Vector3Int NewPositionToMove;
        public Vector3Int OldPosition;
        public GridPositionTuple GridPositions { get; set; }
    }
}