using System;
using System.Collections.Generic;
using App;
using Presentation.Structs;
using UnityEngine;

namespace Presentation
{
    public class BuildingsSelectable : MonoBehaviour
    {
        [SerializeField] private List<BuildingTypeTuple> _buildingsSelector;

        public event Action<BuildingType> OnPlayerWantsToBuyBuilding;
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

        private void BuildingSelected(BuildingType obj)
        {
            OnPlayerWantsToBuyBuilding.Invoke(obj);
        }
    }
}