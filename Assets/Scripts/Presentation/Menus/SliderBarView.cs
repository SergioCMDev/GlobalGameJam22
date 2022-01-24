using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public class SliderBarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Image _fill, _border;

        public void SetMaxValue(float life)
        {
            _slider.maxValue = life;
            _gradient.Evaluate(1f);
            SetValue(life);
        }

        public void SetValue(float energy)
        {
            _slider.value = energy;
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
        }
    }
}