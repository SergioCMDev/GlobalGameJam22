using App.Events;
using UnityEngine;

namespace Services.Popups.Events
{
    [CreateAssetMenu(fileName = "InstantiatePopupEvent",
        menuName = "Events/Popup/InstantiatePopupEvent")]
    public class InstantiatePopupEvent : GameEventScriptable
    {
        public PopupGenerator.PopupType popupType;
    }
}