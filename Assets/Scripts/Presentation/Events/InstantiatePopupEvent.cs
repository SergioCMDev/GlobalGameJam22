using App.Events;
using Presentation.Managers;
using UnityEngine;

namespace Presentation.Events
{
    [CreateAssetMenu(fileName = "InstantiatePopupEvent",
        menuName = "Events/Popup/InstantiatePopupEvent")]
    public class InstantiatePopupEvent : GameEventScriptable
    {
        public PopupType popupType;
    }
}