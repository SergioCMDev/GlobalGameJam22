using System;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public class PlayerHasWonPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Button _buttonRestart, _buttonMainMenu, _buttonContinue;
        public event Action OnGoToMainMenuButtonPressed;
        public event Action OnContinueButtonPressed;
        public event Action OnRestartButtonPressed;

        private void Start()
        {
            _buttonRestart.onClick.AddListener(Restart);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);
            _buttonContinue.onClick.AddListener(Continue);
        }

        private void GoToMainMenu()
        {
            OnClosePopup(gameObject);
            OnGoToMainMenuButtonPressed();
        }

        private void Continue()
        {
            OnClosePopup(gameObject);
            OnContinueButtonPressed();
        }

        private void Restart()
        {
            OnClosePopup(gameObject);
            OnRestartButtonPressed();
        }

        public Action<GameObject> OnClosePopup { get; set; }
    }
}