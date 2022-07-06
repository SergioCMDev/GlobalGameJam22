using System;
using System.Collections.Generic;
using App;
using Presentation.Structs;
using UnityEngine;

namespace Presentation.UI
{
    public class BuildingsSelectable : MonoBehaviour
    {
        [SerializeField] private List<BuildingTypeTuple> _buildingsSelector;

        public event Action<MiltaryBuildingType> OnPlayerWantsToBuyBuilding;
        void Start()
        {
            foreach (var building in _buildingsSelector)
            {
                building.BuildingSelectable.OnBuildingSelected += BuildingSelected;
            }
        }

        private void OnDestroy()
        {
            foreach (var building in _buildingsSelector)
            {
                building.BuildingSelectable.OnBuildingSelected -= BuildingSelected;
            }
        }

        private void BuildingSelected(MiltaryBuildingType obj)
        {
            OnPlayerWantsToBuyBuilding?.Invoke(obj);
        }
    }
}