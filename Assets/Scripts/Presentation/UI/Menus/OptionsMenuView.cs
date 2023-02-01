using System;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Menus
{
    public class OptionsMenuView : MonoBehaviour
    {
        [SerializeField] private Button buttonBack;

        public event Action OnBackButtonPressed = delegate { };

        public void Init()
        {
            buttonBack.onClick.AddListener(ButtonBackPressed);
        }

        public void Hide()
        {
            buttonBack.onClick.RemoveListener(ButtonBackPressed);
            
        }
        

        private void ButtonBackPressed()
        {
            OnBackButtonPressed?.Invoke();
        }
    }
}