#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace App.Events
{
    [CustomEditor(typeof(GameEventScriptable), true)]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            var gameEvent = (GameEventScriptable) target;

            if (!GUILayout.Button("Fire Event")) return;

            gameEvent.Fire();
        }
    }
}
#endif