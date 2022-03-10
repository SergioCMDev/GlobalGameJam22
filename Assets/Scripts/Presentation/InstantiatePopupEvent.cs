using App.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantiatePopupEvent",
    menuName = "Events/Popup/InstantiatePopupEvent")]
public class InstantiatePopupEvent : GameEventScriptable
{
    public PopupType popupType;
}