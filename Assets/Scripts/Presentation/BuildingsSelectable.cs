using System;
using System.Collections.Generic;
using System.Linq;
using Presentation.Structs;
using UnityEngine;

namespace Presentation
{
    public class BuildingsSelectable : MonoBehaviour
    {
        [SerializeField] private List<BuildingPrefabTuple> _buildingsSelector;

        public event Action<BuildingPrefabTuple> OnPlayerWantsToBuyBuilding;
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

        private void BuildingSelected(BuildingSelector obj)
        {
            var buildingPrefabTuple = _buildingsSelector.Single(x => x.BuildingSelectable == obj);
            OnPlayerWantsToBuyBuilding.Invoke(buildingPrefabTuple);
        }
    }
}