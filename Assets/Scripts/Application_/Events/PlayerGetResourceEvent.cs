using Application_.Events;
using UnityEngine;

namespace Application_
{
    [CreateAssetMenu(fileName = "PlayerGetMaterialEvent", menuName = "Events/Resources/PlayerGetMaterialEvent")]
    public class PlayerGetResourceEvent : GameEventScriptable
    {
        public float Quantity;
        public RetrievableResourceType Type;
    }
}