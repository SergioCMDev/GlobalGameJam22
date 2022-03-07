using App.Events;
using Presentation.Managers;
using UnityEngine;
using Utils;

namespace Presentation.MusicEmitter
{
    public class BackgroundSoundEmitter : MonoBehaviour
    {
        private SoundManager _soundManager;
        [SerializeField] private MusicSoundName _backgroundSoundName;
        [SerializeField] private bool _active;

        void Start()
        {
            _soundManager = ServiceLocator.Instance.GetService<SoundManager>();

            if (!_active) return;
            _soundManager.PlayMusic(_backgroundSoundName);
        }

        public void StopMusic()
        {
            _soundManager.StopMusic();
        }
    }
}