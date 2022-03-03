using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "PlayerHasWonEvent", menuName = "Events/Game/PlayerHasWonEvent")]
    public class ShowWinMenuUIEvent : GameEventScriptable
    {
        public int level;
    }
}