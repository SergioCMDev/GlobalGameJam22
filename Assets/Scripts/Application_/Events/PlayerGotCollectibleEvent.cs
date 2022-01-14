using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayerGotCollectibleEvent", menuName = "Events/Level/PlayerGotCollectibleEvent")]
    public class PlayerGotCollectibleEvent : GameEventScriptable
    {
        public GameObject collectible;
    }
}