using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "BuildingReceiveLifeEvent", menuName = "Events/Building/BuildingReceiveLifeEvent")]
    public class BuildingReceiveLifeEvent : GameEventScriptable
    {
        public int Id;
        public float Life;
        
    }
}