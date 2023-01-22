using System;
using DG.Tweening;
using UnityEngine;

namespace Presentation.LoadingScene
{
    public class CanvasFader : MonoBehaviour
    {
        [SerializeField] private Transform upImage, downImage;
        [SerializeField] private float upImageMovementToDown, downImageMovementToUp;
        public event Action OnFadeCompleted;
        public event Action OnUnfadeCompleted;
        [SerializeField] private float _fadeDuration, _unfadeDuration;

        // [SerializeField] private Image _fader;
        private Sequence sequence;
        

        public void ActivateFader()
        {
            var destinationUpImage = upImage.position + Vector3.down * upImageMovementToDown;
            var destinationDownImage = downImage.position - Vector3.down * downImageMovementToUp;
            var upTween = upImage.DOMoveY(destinationUpImage.y, _fadeDuration);
            var downTween = downImage.DOMoveY(destinationDownImage.y, _fadeDuration);
            sequence = DOTween.Sequence();

            if (sequence != null)
            {
                sequence.Join(upTween);
                sequence.Join(downTween);
            }

            Debug.Log("Activamos Fade");
            // var fader = _fader.DOFade(1, _fadeDuration);
            sequence.onComplete += InvokeFadeCompleted;
            sequence.onUpdate +=  ()=>{
               Debug.Log("F"); 
            }; 
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
            var destinationUpImage = upImage.position - Vector3.down * upImageMovementToDown;
            var destinationDownImage = downImage.position + Vector3.down * downImageMovementToUp;
            var upTween = upImage.DOMoveY(destinationUpImage.y, _unfadeDuration);
            var downTween = downImage.DOMoveY(destinationDownImage.y, _unfadeDuration);
            sequence = DOTween.Sequence();
            if (sequence != null)
            {
                sequence.Join(upTween);
                sequence.Join(downTween);
            }

            Debug.Log("Desactivamos Fade");
            // var fader = _fader.DOFade(1, _fadeDuration);
            sequence.onComplete += InvokeUnFadeCompleted;
            sequence.onUpdate +=  ()=>{
                Debug.Log("F1"); 
            }; 
            sequence.Play();
        }
        
        private void InvokeUnFadeCompleted()
        {
            sequence.onComplete -= InvokeUnFadeCompleted; 
            OnUnfadeCompleted.Invoke();
        }

        public void StatusImages(bool status)
        {
            upImage.gameObject.SetActive(status);
            downImage.gameObject.SetActive(status);
        }
    }
}