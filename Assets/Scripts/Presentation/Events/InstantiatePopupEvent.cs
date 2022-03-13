using App.Events;
using UnityEngine;

namespace Presentation
{
    [CreateAssetMenu(fileName = "InstantiatePopupEvent",
        menuName = "Events/Popup/InstantiatePopupEvent")]
    public class InstantiatePopupEvent : GameEventScriptable
    {
        public PopupType popupType;
    }
}