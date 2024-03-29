using System;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Managers
{
    public class LevelSelectorView : MonoBehaviour
    {
        [SerializeField] private Button buttonBack, leftButton, rightButton, playButton;
        [SerializeField] private Image levelImage;
        public event Action OnStartLevelSelected;
        public event Action OnButtonBackIsPressed;
        public event Action OnLeftButtonIsPressed;
        public event Action OnRightButtonIsPressed;
        
        private void Start()
        {
            buttonBack.onClick.AddListener(() => OnButtonBackIsPressed?.Invoke());
            leftButton.onClick.AddListener(ChangeLevelToLeft);
            rightButton.onClick.AddListener(ChangeLevelToRight);
            playButton.onClick.AddListener(PlayLevel);
        }

        private void OnDestroy()
        {
            buttonBack.onClick.RemoveListener(() => OnButtonBackIsPressed?.Invoke());
            leftButton.onClick.RemoveListener(ChangeLevelToLeft);
            rightButton.onClick.RemoveListener(ChangeLevelToRight);
            playButton.onClick.RemoveListener(PlayLevel);
        }

        private void ChangeLevelToLeft()
        {
            OnLeftButtonIsPressed?.Invoke();
        }

        private void ChangeLevelToRight()
        {
            OnRightButtonIsPressed?.Invoke();
        }


        private void PlayLevel()
        {
            OnStartLevelSelected?.Invoke();
        }
        
        public void SetLevelImage(Sprite levelSprite)
        {
            levelImage.sprite = levelSprite;
        }
    }
}