using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayerHasCompletedLevelEvent", menuName = "Events/Level/PlayerHasCompletedLevelEvent")]
    public class PlayerHasCompletedLevelEvent : GameEventScriptable
    {
        public int level;
    }
}