using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "PlayerGetMaterialEvent", menuName = "Events/Resources/PlayerGetMaterialEvent")]
    public class PlayerGetResourceEvent : GameEventScriptable
    {
        public float Quantity;
        public RetrievableResourceType Type;
    }
}