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
            _button.onClick.AddListener(BuildingSelected1);
        }

        private void BuildingSelected1()
        {
            OnBuildingSelected.Invoke(this);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(BuildingSelected);
        }

        protected abstract void BuildingSelected();

        public abstract void MakeSound();
    }
}