using System;
using TMPro;
using UnityEngine;

namespace Presentation.UI.Menus
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(String text)
        {
            _text.SetText(text);
        }
        
        public void SetColorText(Color color)
        {
            _text.color = color;
        }
    }
}