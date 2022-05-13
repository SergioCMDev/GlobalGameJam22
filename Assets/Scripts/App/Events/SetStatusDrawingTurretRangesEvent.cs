using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "ShowTurretRangesEvent", menuName = "Events/Level/ShowTurretRangesEvent")]
    public class SetStatusDrawingTurretRangesEvent : GameEventScriptable
    {
        public bool drawingStatus;
    }
}