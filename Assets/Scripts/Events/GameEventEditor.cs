#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
#endif