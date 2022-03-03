using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "PlayerGotCollectibleEvent", menuName = "Events/Level/PlayerGotCollectibleEvent")]
    public class PlayerGotCollectibleEvent : GameEventScriptable
    {
        public GameObject collectible;
    }
}