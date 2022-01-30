using System;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class WinLoseMenuView : MonoBehaviour
    {
        [SerializeField] private Image _winImage, _loseImage;
        [SerializeField] private Button _buttonRestart, _buttonMainMenu, _buttonContinue;
        [SerializeField] private GameObject _buttonsParent;
        public event Action OnGoToMainMenuButtonPressed;
        public event Action OnContinueButtonPressed;
        public event Action OnRestartButtonPressed;

        private void Start()
        {
            _winImage.gameObject.SetActive(false);
            _loseImage.gameObject.SetActive(false);

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

        public void ShowLoseImage()

        {
            _buttonsParent.gameObject.SetActive(true);
            _loseImage.gameObject.SetActive(true);
        }

        public void ShowWinImage()
        {
            _buttonsParent.gameObject.SetActive(true);
            _winImage.gameObject.SetActive(true);
        }
    }
}