using System;
using System.Collections;
using DG.Tweening;
using Presentation.Interfaces;
using Presentation.UI.Menus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class NewRoundPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private TextMeshProUGUI incomingAttackText;
        [SerializeField] private CanvasGroup canvasPanel;
        [SerializeField] private float fadeDuration = 0.4f;
        private Sequence sequence;
        private bool fadeToZero;
        private float textAlphaToReach;

        public Action<GameObject> HasToClosePopup { get; set; }
        public Action PopupHasBeenClosed { get; set; }

        public void Init(float timeToShow)
        {
            Timer timer = new Timer();
            timer.Init(timeToShow);
            StartCoroutine(timer.CountTime());
            textAlphaToReach = 0;
            FadePanel(textAlphaToReach);
            fadeToZero = true;
            timer.OnTimerEnds += ClosePopup;
        }

        private void FadePanel(float alpha)
        {
            var tween = canvasPanel.DOFade(alpha, fadeDuration).SetEase(Ease.InOutSine);
            tween.OnComplete(RevertFade);
        }

        private void RevertFade()
        {
            fadeToZero = !fadeToZero;
            textAlphaToReach = fadeToZero ? 0 : 1f;
            FadePanel(textAlphaToReach);
        }

        private void ClosePopup()
        {
            HasToClosePopup.Invoke(gameObject);
        }
    }
}