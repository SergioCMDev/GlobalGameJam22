using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayerHasWonEvent", menuName = "Events/Game/PlayerHasWonEvent")]
    public class ShowWinMenuUIEvent : GameEventScriptable
    {
        public int level;
    }
}