using System;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public class SliderBarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Image _fill, _border;
        public event Action OnSliderReachZero;

        public void SetMaxValue(float maxValue)
        {
            _slider.maxValue = maxValue;
            _gradient.Evaluate(1f);
            SetValue(maxValue);
        }

        public void SetValue(float value)
        {
            _slider.value = value;
            if (_slider.value <= 0)
            {
                OnSliderReachZero?.Invoke();
            }

            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
        }

        private Slider.SliderEvent CheckValue()
        {
            throw new System.NotImplementedException();
        }
    }
}