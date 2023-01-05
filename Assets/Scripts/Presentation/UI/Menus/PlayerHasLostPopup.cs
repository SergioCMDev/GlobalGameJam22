using System;
using Services.Popups.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Menus
{
    public class PlayerHasLostPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Button _buttonRestart, _buttonMainMenu;
        public event Action OnGoToMainMenuButtonPressed;
        public event Action OnRestartButtonPressed;
        public Action<GameObject> HasToClosePopup { get; set; }
        public Action PopupHasBeenClosed { get; set; }

        private void Start()
        {
            _buttonRestart.onClick.AddListener(Restart);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);
        }

        private void GoToMainMenu()
        {
            HasToClosePopup(gameObject);
            OnGoToMainMenuButtonPressed?.Invoke();
        }

        private void Restart()
        {
            HasToClosePopup(gameObject);
            OnRestartButtonPressed?.Invoke();
        }
    }
}