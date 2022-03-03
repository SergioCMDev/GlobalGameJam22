using UnityEngine;

namespace App.Events
{
    [CreateAssetMenu(fileName = "PlaySFXEvent", menuName = "Events/Sound/PlaySFXEvent")]
    public class PlaySFXEvent : GameEventScriptable
    {
        public SfxSoundName soundName;
    }
}