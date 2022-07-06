using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "UpdateUIResourcesEvent", menuName = "Events/Resources/UpdateUIResourcesEvent")]
    public class UpdateUIResourcesEvent : GameEventScriptable
    {
        public int previousQuantity;
        public int currentQuantity;
    }
}