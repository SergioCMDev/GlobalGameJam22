using System;
using Presentation.Building;
using UnityEngine;

namespace Presentation
{
    public class MilitaryBuildingPlacementSetter : MonoBehaviour
    {
        [SerializeField] private PlacerBuildingView _chooserCanvas;

        public event Action OnBuildingTriesToTakePlace, OnCancelTakingPlace;

        private void Awake()
        {
            _chooserCanvas.gameObject.SetActive(false);
            _chooserCanvas.OnCancelTakingPlace += CancelTakingPlace;
            _chooserCanvas.OnBuildingTriesToTakePlace += BuildingTriesToTakePlace;
        }

        public void SetStatusChooserCanvas(bool status)
        {
            _chooserCanvas.gameObject.SetActive(status);
        }

        //TODO FIND WHERE TO PUT THIS, A NEW UPPER MANAGER? 
        private void CancelTakingPlace()
        {
            OnCancelTakingPlace();
        }

        private void BuildingTriesToTakePlace()
        {
            OnBuildingTriesToTakePlace();
        }
    }
}