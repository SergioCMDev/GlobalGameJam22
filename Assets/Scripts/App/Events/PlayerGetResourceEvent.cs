using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "PlayerGetMaterialEvent", menuName = "Events/Resources/PlayerGetMaterialEvent")]
    public class PlayerGetResourceEvent : GameEventScriptable
    {
        public int Quantity;
        public RetrievableResourceType Type;
    }
}