using App.Events;
using Presentation.Managers;
using UnityEngine;
using Utils;

namespace Presentation.MusicEmitter
{
    public class BackgroundSoundEmitter : MonoBehaviour
    {
        private SoundPlayer _soundPlayer;
        [SerializeField] private MusicSoundName _backgroundSoundName;
        [SerializeField] private bool _active;

        void Start()
        {
            _soundPlayer = ServiceLocator.Instance.GetService<SoundPlayer>();

            if (!_active) return;
            _soundPlayer.PlayMusic(_backgroundSoundName);
        }

        public void StopMusic()
        {
            _soundPlayer.StopMusic();
        }
    }
}