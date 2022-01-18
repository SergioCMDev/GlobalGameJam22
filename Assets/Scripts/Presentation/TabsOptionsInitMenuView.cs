using System;
using Presentation.Input;
using Presentation.Menus;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Presentation.Views
{
    public class TabsOptionsInitMenuView : ViewWithHorizontalButtonsBase
    {
        [SerializeField] private ButtonView _buttonViewControls, _buttonViewVideo, _buttonViewAudio, _buttonViewMisc;
        private Button _buttonControls, _buttonVideo, _buttonAudio, _buttonMisc;

        public event Action OnPlayerPressButtonControls;
        public event Action OnPlayerPressButtonVideo;
        public event Action OnPlayerPressButtonAudio;
        public event Action OnPlayerPressButtonMisc;

        void Awake()
        {
            readInputPlayer = ServiceLocator.Instance.GetService<ReadInputPlayer>();
            readInputPlayer.OnPlayerPressHorizontalAxisButtons += PlayerPressXAxisButtons;
            // readInputPlayer.OnPlayerPressEnterButton += HandlePlayerPressEnterButton;

            _buttonControls = _buttonViewControls.GetComponent<Button>();
            _buttonVideo = _buttonViewVideo.GetComponent<Button>();
            _buttonAudio = _buttonViewAudio.GetComponent<Button>();
            _buttonMisc = _buttonViewMisc.GetComponent<Button>();

            interactableButtonsViews.Add(_buttonViewControls);
            interactableButtonsViews.Add(_buttonViewVideo);
            interactableButtonsViews.Add(_buttonViewAudio);
            interactableButtonsViews.Add(_buttonViewMisc);

            interactableButtons.Add(_buttonControls);
            interactableButtons.Add(_buttonVideo);
            interactableButtons.Add(_buttonAudio);
            interactableButtons.Add(_buttonMisc);


            _buttonControls.onClick.AddListener(ShowControls);
            _buttonVideo.onClick.AddListener(ShowVideo);
            _buttonAudio.onClick.AddListener(ShowAudio);
            _buttonMisc.onClick.AddListener(ShowMisc);
        }

        private void Start()
        {
            // SetSelectedButton(0);
        }

        private void ShowMisc()
        {
            OnPlayerPressButtonMisc.Invoke();

            SetSelectedButton(_buttonViewMisc.IdOrderButton);
            Debug.Log("SHOW MISC");
        }

        private void ShowAudio()
        {
            SetSelectedButton(_buttonViewAudio.IdOrderButton);
            OnPlayerPressButtonAudio.Invoke();


            Debug.Log("SHOW AUDIO");
        }

        private void ShowVideo()
        {
            SetSelectedButton(_buttonViewVideo.IdOrderButton);
            OnPlayerPressButtonVideo.Invoke();


            Debug.Log("SHOW VIDEO");
        }

        public void ShowControls()
        {
            SetSelectedButton(_buttonViewControls.IdOrderButton);
            OnPlayerPressButtonControls.Invoke();

            Debug.Log("SHOW CONTROLS");
        }
    }
}