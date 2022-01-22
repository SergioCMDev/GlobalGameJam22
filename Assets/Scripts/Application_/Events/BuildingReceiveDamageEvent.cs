using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "BuildingReceiveDamageEvent", menuName = "Events/Building/BuildingReceiveDamageEvent")]
    public class BuildingReceiveDamageEvent : GameEventScriptable
    {
        public int Id;
        public float Damage;
        
    }
}