using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public class PlayerHasLostPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Image _loseImage;
        [SerializeField] private Button _buttonRestart, _buttonMainMenu;
        [SerializeField] private GameObject _buttonsParent;
        public event Action OnGoToMainMenuButtonPressed;
        public event Action OnRestartButtonPressed;
        public Action<GameObject> OnClosePopup { get; set; }

        private void Start()
        {
            _loseImage.gameObject.SetActive(false);
            _buttonRestart.onClick.AddListener(Restart);
            _buttonMainMenu.onClick.AddListener(GoToMainMenu);
            _buttonsParent.gameObject.SetActive(false);
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