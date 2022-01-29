using Application_.Events;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveBuildingEvent", menuName = "Events/Building/SaveBuildingEvent")]
public class SaveBuildingEvent : GameEventScriptable
{
    public GameObject Instance;
}