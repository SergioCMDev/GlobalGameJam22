using System;
using Services.Popups.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Menus
{
    public class PlayerHasWonPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Button _buttonRestart, _buttonMainMenu, _buttonContinue;
        public event Action OnGoToMainMenuButtonPressed;
        public event Action OnContinueButtonPressed;
        public event Action OnRestartButtonPressed;
        public Action<GameObject> HasToClosePopup { get; set; }
        public Action PopupHasBeenClosed { get; set; }
        private void Start()
        {
            _buttonRestart.onClick.AddListener(Restart);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);
            _buttonContinue.onClick.AddListener(Continue);
        }

        private void GoToMainMenu()
        {
            HasToClosePopup(gameObject);
            OnGoToMainMenuButtonPressed?.Invoke();
        }

        private void Continue()
        {
            HasToClosePopup(gameObject);
            OnContinueButtonPressed?.Invoke();
        }

        private void Restart()
        {
            HasToClosePopup(gameObject);
            OnRestartButtonPressed?.Invoke();
        }


    }
}