using System;
using Services.Popups.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    public class PausePopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Button quitLevel, restartLevel, resumeLevel;

        public event Action OnQuitLevelClicked, OnRestartLevelClicked;
        public event Action<GameObject>OnResumeLevelClicked;

        private void Start()
        {
            quitLevel.onClick.AddListener(QuitLevel);
            restartLevel.onClick.AddListener(RestartLevel);
            resumeLevel.onClick.AddListener(ResumeLevel);
        }

        private void ResumeLevel()
        {
            OnResumeLevelClicked?.Invoke(gameObject);
        }

        private void RestartLevel()
        {
            OnRestartLevelClicked?.Invoke();
        }

        private void QuitLevel()
        {
            OnQuitLevelClicked?.Invoke();
        }

        public Action<GameObject> HasToClosePopup { get; set; }
        public Action PopupHasBeenClosed { get; set; }
    }
}