using System;

namespace Presentation.Infrastructure
{
    public class MilitaryBuildingPlacementSetter
    {
        private PlacerBuildingView _chooserCanvas;
        public event Action OnBuildingTriesToTakePlace, OnCancelTakingPlace;

        public void Init(PlacerBuildingView chooserCanvas)
        {
            _chooserCanvas = chooserCanvas;
            _chooserCanvas.gameObject.SetActive(false);
        }

        public void AddListeners()
        {
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