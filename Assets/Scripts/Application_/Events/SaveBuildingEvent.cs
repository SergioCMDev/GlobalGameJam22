using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "SaveBuildingEvent", menuName = "Events/Building/SaveBuildingEvent")]
    public class SaveBuildingEvent : GameEventScriptable
    {
        public GameObject Instance;
    }
}
