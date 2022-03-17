using System;
using App;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    public class BuildingSelector : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private BuildingType _buildingType;
        public event Action<BuildingType> OnBuildingSelected;

        public BuildingType BuildingType => _buildingType;

        void Start()
        {
            _button.onClick.AddListener(BuildingSelected);
        }

        private void BuildingSelected()
        {
            OnBuildingSelected.Invoke(BuildingType);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(BuildingSelected);
        }
        
    }
}