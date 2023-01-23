using System;
using DG.Tweening;
using UnityEngine;

namespace Presentation.LoadingScene
{
    public class CanvasFader : MonoBehaviour
    {
        [SerializeField] private Transform upImage, downImage;
        [SerializeField] private float _fadeDuration, _unfadeDuration;

        private RectTransform upImageRectTransform, downImageRectTransform;

        public event Action OnFadeCompleted;
        public event Action OnReverseFadeCompleted;

        private Sequence sequence;
        private Vector2 originalPositionUpperImage, originalPositionDownImage;

        private void Start()
        {
            upImageRectTransform = upImage.GetComponent<RectTransform>();
            downImageRectTransform = downImage.GetComponent<RectTransform>();
            originalPositionDownImage = downImageRectTransform.anchoredPosition;
            originalPositionUpperImage = upImageRectTransform.anchoredPosition;
        }

        public void ActivateFader()
        {
            var upTween = upImage.DOLocalMoveY(0, _fadeDuration);
            var downTween = downImage.DOMoveY(0, _fadeDuration);
            sequence = DOTween.Sequence();

            if (sequence != null)
            {
                sequence.Join(upTween);
                sequence.Join(downTween);
            }

            Debug.Log("Activamos Fade");
            sequence.onComplete += InvokeFadeCompleted;
            sequence.onUpdate += () => { Debug.Log("F"); };
            sequence.Play();
        }

        private void InvokeFadeCompleted()
        {
            sequence.onComplete -= InvokeFadeCompleted;
            OnFadeCompleted.Invoke();
        }


        public void DeactivateFader()
        {
            Debug.Log("Desactivamos Fade");
            var valueOfDestinationImages = upImageRectTransform.anchoredPosition - Vector2.up * originalPositionUpperImage;

            var upTween = upImage.DOLocalMoveY(-valueOfDestinationImages.y, _unfadeDuration);
            var downTween = downImage.DOLocalMoveY(valueOfDestinationImages.y, _unfadeDuration);
            sequence = DOTween.Sequence();

            if (sequence != null)
            {
                sequence.Join(upTween);
                sequence.Join(downTween);
            }

            Debug.Log("Desactivamos Fade");

            sequence.onComplete += InvokeUnFadeCompleted;
            sequence.onUpdate += () => { Debug.Log("F1"); };
            sequence.Play();
        }

        private void InvokeUnFadeCompleted()
        {
            sequence.onComplete -= InvokeUnFadeCompleted;
            OnReverseFadeCompleted.Invoke();
        }

        public void StatusImages(bool status)
        {
            upImage.gameObject.SetActive(status);
            downImage.gameObject.SetActive(status);
        }
    }
}