using System;
using Presentation.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public class PlayerHasLostPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Button _buttonRestart, _buttonMainMenu;
        public event Action OnGoToMainMenuButtonPressed;
        public event Action OnRestartButtonPressed;
        public Action<GameObject> OnClosePopup { get; set; }

        private void Start()
        {
            _buttonRestart.onClick.AddListener(Restart);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);
        }

        private void GoToMainMenu()
        {
            OnClosePopup(gameObject);
            OnGoToMainMenuButtonPressed();
        }


        private void Restart()
        {
            OnClosePopup(gameObject);
            OnRestartButtonPressed();
        }

    }
}