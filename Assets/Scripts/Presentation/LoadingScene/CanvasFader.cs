using System;
using DG.Tweening;
using Plugins.DOTween.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.LoadingScene
{
    public class CanvasFader : MonoBehaviour
    {
        public event Action OnFadeCompleted;
        public event Action OnUnfadeCompleted;
        [SerializeField] private float _fadeDuration, _unfadeDuration = 0.5f;

        [SerializeField] private Image _fader;

        public void ActivateFader()
        {
            Debug.Log("Activamos Fade");
            var fader = _fader.DOFade(1, _fadeDuration);
            fader.Play();
            fader.onComplete += () => OnFadeCompleted.Invoke();
        }


        public void DeactivateFader()
        {
            Debug.Log("Desactivamos Fade");
            var fader = _fader.DOFade(0, _unfadeDuration);
            fader.Play();
            fader.onComplete += () => OnUnfadeCompleted.Invoke();
        }
    }
}