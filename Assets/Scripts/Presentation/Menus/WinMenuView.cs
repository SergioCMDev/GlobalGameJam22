using System;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public class WinMenuView : MonoBehaviour
    {
        [SerializeField] private Image _winImage;
        [SerializeField] private Button _buttonRestart, _buttonMainMenu, _buttonContinue;
        [SerializeField] private GameObject _buttonsParent;
        public event Action OnGoToMainMenuButtonPressed;
        public event Action OnContinueButtonPressed;
        public event Action OnRestartButtonPressed;

        private void Start()
        {
            _winImage.gameObject.SetActive(false);
            _buttonContinue.gameObject.SetActive(false);
            _buttonRestart.onClick.AddListener(Restart);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);
            _buttonContinue.onClick.AddListener(Continue);
            _buttonsParent.gameObject.SetActive(false);
        }

        private void GoToMainMenu()
        {
            OnGoToMainMenuButtonPressed();
        }

        private void Continue()
        {
            OnContinueButtonPressed();
        }

        private void Restart()
        {
            OnRestartButtonPressed();
        }
    }
}