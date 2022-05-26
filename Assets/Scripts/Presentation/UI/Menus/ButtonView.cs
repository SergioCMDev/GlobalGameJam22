using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Menus
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private int _idOrderButton;

        public int IdOrderButton => _idOrderButton;

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