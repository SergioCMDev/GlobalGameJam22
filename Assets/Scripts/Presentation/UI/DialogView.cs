using System;
using System.Collections;
using Presentation.InputPlayer;
using TMPro;
using UnityEngine;

namespace Presentation.UI
{
    public class DialogView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _timeBetweenChars;

        // public SelectorView SelectorView;
        private ReadInputPlayer _readInputPlayer;

        public event Action OnWritingTextHasEnded = delegate { };

        private string _textToWrite;
        private Coroutine _coroutine;

        public void Init(ReadInputPlayer readInputPlayer)
        {
            _readInputPlayer = readInputPlayer;
        }

        private void EndWritingCoroutine()
        {
            // _readInputPlayer.OnPlayerPressLeftButtonMouseOnDialog -= EndWritingCoroutine;

            StopCoroutine(_coroutine);
            _text.SetText(_textToWrite);
            OnWritingTextHasEnded.Invoke();
        }

        IEnumerator WriteText(string text)
        {
            _text.SetText(string.Empty);
            _textToWrite = text;

            foreach (var charToWrite in _textToWrite.ToCharArray())
            {
                _text.SetText(_text.text + charToWrite);
                yield return new WaitForSeconds(_timeBetweenChars);
            }

            OnWritingTextHasEnded.Invoke();
            yield return null;
        }

        public void SetText(string text)
        {
            // _readInputPlayer.OnPlayerPressLeftButtonMouseOnDialog += EndWritingCoroutine;

            _coroutine = StartCoroutine(WriteText(text));
        }
    }
}