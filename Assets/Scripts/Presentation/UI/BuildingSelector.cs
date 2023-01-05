using System;
using App;
using App.Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    public class BuildingSelector : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private MilitaryBuildingType militaryBuildingType;
        public event Action<MilitaryBuildingType> OnBuildingSelected;

        public MilitaryBuildingType MilitaryBuildingType => militaryBuildingType;

        void Start()
        {
            _button.onClick.AddListener(BuildingSelected);
        }

        private void BuildingSelected()
        {
            OnBuildingSelected.Invoke(MilitaryBuildingType);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(BuildingSelected);
        }
        
    }
}