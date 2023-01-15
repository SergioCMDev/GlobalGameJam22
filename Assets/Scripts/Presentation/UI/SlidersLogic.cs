using System;
using System.Collections;
using System.Collections.Generic;
using Presentation.UI.Menus;
using UnityEngine;

public class SlidersLogic : MonoBehaviour
{
    private bool _stopSlider;
    private SliderBarView _currentSliderBarView;
    [SerializeField] private SliderBarView _defensiveTimer;
    private float _remainingTime;

    void Start()
    {
        _stopSlider = false;
    }
    
    public void SetBuilderTimerInitialValue(float time, Action onTimerHasEnded)
    {
        _defensiveTimer.SetMaxValue(time);

        _remainingTime = time;
        _currentSliderBarView = _defensiveTimer;
        _currentSliderBarView.OnSliderReachZero += () => onTimerHasEnded?.Invoke();
    }

    public void SetDefensiveTimerInitialValue(float time, Action onTimerHasEnded)
    {
        _defensiveTimer.SetMaxValue(time);

        _remainingTime = time;
        _currentSliderBarView = _defensiveTimer;
        _currentSliderBarView.OnSliderReachZero += () => onTimerHasEnded?.Invoke();
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
        _currentSliderBarView.OnSliderReachZero -= StopTimerLogic;
        StopCoroutine(StartSliderTimer());
    }
    
    public void InitTimerLogic()
    {
        StopTimerLogic();
        _currentSliderBarView.OnSliderReachZero += StopTimerLogic;
        StartCoroutine(StartSliderTimer());
    }
}
