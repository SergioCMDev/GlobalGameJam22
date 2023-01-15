using System;
using System.Collections;
using Presentation.UI.Menus;
using UnityEngine;

namespace Presentation.UI
{
    public class SliderLogic : MonoBehaviour
    {
        private bool _stopSlider;
        [SerializeField] private SliderBarView slider;
        private SliderBarView _currentSliderBarView;
        private float _remainingTime;
        private Action _onTimerHasEnded;

        public void SetSliderTimerInitialValues(float time, Action onTimerHasEnded)
        {
            _remainingTime = time;
            _currentSliderBarView = slider;
            _currentSliderBarView.SetMaxValue(time);
            _onTimerHasEnded = onTimerHasEnded;
            _currentSliderBarView.OnSliderReachZero += OnSliderReachZero;
        }

        private void OnSliderReachZero()
        {
            _currentSliderBarView.OnSliderReachZero -= OnSliderReachZero;
            StopTimerLogic();
            _onTimerHasEnded?.Invoke();
        }

        private IEnumerator StartSliderTimer()
        {
            do
            {
                _remainingTime -= Time.deltaTime;
                _currentSliderBarView.SetValue(_remainingTime);
                yield return null;
            } while (_remainingTime > 0 & !_stopSlider);
        }

        public void StopTimerLogic()
        {
            _stopSlider = true;
            _currentSliderBarView.enabled = false;
            StopCoroutine(StartSliderTimer());
        }

        public void InitTimerLogic()
        {
            EnableSlider();
            StartCoroutine(StartSliderTimer());
        }

        private void EnableSlider()
        {
            _stopSlider = false;
            _currentSliderBarView.enabled = true;
        }
    }
}