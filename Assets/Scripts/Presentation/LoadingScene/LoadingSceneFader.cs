using TMPro;
using UnityEngine;

namespace Presentation.LoadingScene
{
    public class LoadingSceneFader : CanvasFader
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(string toString)
        {
            _text.SetText(toString);
        }

        public void DeactivateUI()
        {
            _text.gameObject.SetActive(false);
        }
    }
}