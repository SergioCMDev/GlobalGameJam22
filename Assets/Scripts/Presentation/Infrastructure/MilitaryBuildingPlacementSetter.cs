using System;
using UnityEngine;

namespace Presentation.Infrastructure
{
    //TODO REMOVE MONOBEHAVIOUR
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
            OnCancelTakingPlace?.Invoke();
        }

        private void BuildingTriesToTakePlace()
        {
            OnBuildingTriesToTakePlace?.Invoke();
        }
    }
}