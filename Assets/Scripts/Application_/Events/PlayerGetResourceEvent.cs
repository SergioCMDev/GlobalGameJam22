using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayerGetMaterialEvent", menuName = "Events/Resources/PlayerGetMaterialEvent")]
    public class PlayerGetResourceEvent : GameEventScriptable
    {
        public float Quantity;
        public RetrievableResourceType Type;
    }
}