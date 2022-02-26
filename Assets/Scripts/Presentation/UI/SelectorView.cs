using System.Collections.Generic;
using UnityEngine;

namespace Presentation.UI
{
    public class SelectorView : MonoBehaviour
    {
        [SerializeField] private GameObject _optionViewPrefab;
        
        private int indexSelection;
        private List<OptionView> _optionsInstances;

        private void Awake()
        {
            _optionsInstances = new List<OptionView>();
            indexSelection = 0;
        }

        public void SetOptions(List<string> options)
        {
            foreach (var option in options)
            {
                var optionInstance = Instantiate(_optionViewPrefab, transform);
                var optionsViewInstance = optionInstance.GetComponent<OptionView>();
                optionsViewInstance.SetText(option);
                _optionsInstances.Add(optionsViewInstance);
                optionInstance.gameObject.SetActive(false);
            }

            Debug.Log($"[Node Info] Initial Selection {options[indexSelection]}");
        }

        public void ShowOptions()
        {
            foreach (var option in _optionsInstances)
            {
                option.gameObject.SetActive(true);
            }

            _optionsInstances[indexSelection].Select();
        }

        public void ChangeSelection()
        {
            _optionsInstances[indexSelection].Deselect();
            indexSelection++;
            if (indexSelection >= _optionsInstances.Count)
            {
                indexSelection = 0;
            }

            _optionsInstances[indexSelection].Select();
            Debug.Log($"[Node Info] Selection has Changed to -> {_optionsInstances[indexSelection].GetText()}");
        }

        public OptionView GetSelection()
        {
            return _optionsInstances[indexSelection];
        }

        public void CleanOptions()
        {
            foreach (var option in _optionsInstances)
            {
                option.Deselect();
                Destroy(option.gameObject);
            }

            _optionsInstances.Clear();
        }
    }
}