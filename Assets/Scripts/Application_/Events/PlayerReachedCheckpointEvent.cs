using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayerReachedCheckpointEvent", menuName = "Events/Level/PlayerReachedCheckpointEvent")]
    public class PlayerReachedCheckpointEvent : GameEventScriptable
    {
        public int Id;
    }
}