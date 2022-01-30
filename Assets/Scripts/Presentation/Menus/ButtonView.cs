using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public class ButtonView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private Color _originalColor;
        [SerializeField] private int _idOrderButton;
        [SerializeField] private Image _selector;

        public int IdOrderButton => _idOrderButton;

        private void Awake()
        {
            _selector.gameObject.SetActive(false);

            _originalColor = _text.color;
        }

        public void SetText(String text)
        {
            _text.SetText(text);
        }

        public void StartSparkle()
        {
            _text.color = Color.white;
            // _selector.gameObject.SetActive(true);
        }

        public void EndSparkle()
        {
            _selector.gameObject.SetActive(false);
            _text.color = _originalColor;
        }

        public void SetColorText(Color color)
        {
            _text.color = color;
        }
    }
}