using TMPro;
using UnityEngine;

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