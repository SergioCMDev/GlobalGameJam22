using System;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Building
{
    public class PlacerBuildingView : MonoBehaviour
    {
        [SerializeField] private Button _buttonTakePlace, _buttonCancel;

        public event Action OnBuildingTriesToTakePlace, OnCancelTakingPlace;

        private void Start()
        {
            _buttonTakePlace.onClick.AddListener(TakePlace);
            _buttonCancel.onClick.AddListener(Cancel);
        }

        private void TakePlace()
        {
            OnBuildingTriesToTakePlace();
        }

        private void Cancel()
        {
            OnCancelTakingPlace();
        }
    }
}