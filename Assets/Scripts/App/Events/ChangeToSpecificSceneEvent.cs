using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "ChangeToSpecificSceneEvent", menuName = "Events/Game/ChangeToSpecificSceneEvent")]
    public class ChangeToSpecificSceneEvent : GameEventScriptable
    {
        public string SceneName;
    }
}