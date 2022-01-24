using System;
using Application_;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public abstract class BuildingSelector : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private BuildingType _buildigType;
        public event Action<BuildingSelector> OnBuildingSelected;

        public BuildingType BuildingType => _buildigType;

        void Start()
        {
            _button.onClick.AddListener(BuildingSelected);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(BuildingSelected);
        }

        private void BuildingSelected()
        {
            OnBuildingSelected.Invoke(this);
        }

        public abstract void MakeSound();
    }
}