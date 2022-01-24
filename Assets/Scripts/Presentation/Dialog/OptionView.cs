using TMPro;
using UnityEngine;

namespace Presentation.Dialog
{
    public class OptionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _optionText;

        public void SetText(string textOption)
        {
            _optionText.SetText(textOption);
        }

    

        public string GetText()
        {
            return _optionText.text;
        }

        public void Select()
        {
            _optionText.color = Color.magenta;
        }

        public void Deselect()
        {
            _optionText.color = Color.black;
        }
    }
}