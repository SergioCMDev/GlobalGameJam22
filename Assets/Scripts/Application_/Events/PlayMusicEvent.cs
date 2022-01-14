using UnityEngine;

namespace Application_.Events
{
    [CreateAssetMenu(fileName = "PlayMusicEvent", menuName = "Events/Sound/PlayMusicEvent")]
    public class PlayMusicEvent : GameEventScriptable
    {
        public MusicSoundName soundName;
    }
}