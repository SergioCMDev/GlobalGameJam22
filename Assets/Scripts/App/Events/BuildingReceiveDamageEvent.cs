using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "BuildingReceiveDamageEvent", menuName = "Events/Building/BuildingReceiveDamageEvent")]
    public class BuildingReceiveDamageEvent : GameEventScriptable
    {
        public int Id;
        public float Damage;
        
    }
}