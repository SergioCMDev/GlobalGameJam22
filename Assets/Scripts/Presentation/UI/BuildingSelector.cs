using System;
using App;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    public class BuildingSelector : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private MiltaryBuildingType miltaryBuildingType;
        public event Action<MiltaryBuildingType> OnBuildingSelected;

        public MiltaryBuildingType MiltaryBuildingType => miltaryBuildingType;

        void Start()
        {
            _button.onClick.AddListener(BuildingSelected);
        }

        private void BuildingSelected()
        {
            OnBuildingSelected.Invoke(MiltaryBuildingType);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(BuildingSelected);
        }
        
    }
}