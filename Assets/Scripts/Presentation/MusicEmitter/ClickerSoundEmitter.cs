using App.Events;
using Presentation.Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Presentation.MusicEmitter
{
    public class ClickerSoundEmitter : MonoBehaviour
    {
        private SoundManager _soundManager;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlayClickSound);
        }

        private void PlayClickSound()
        {
            _soundManager.PlaySfx(SfxSoundName.Press_Button);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(PlayClickSound);
        }

        void Start()
        {
            _soundManager = ServiceLocator.Instance.GetService<SoundManager>();
        }
    }
}