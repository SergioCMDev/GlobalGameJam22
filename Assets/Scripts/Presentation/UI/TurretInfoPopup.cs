using System;
using App;
using Presentation.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    public class TurretInfoPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Button _buyButton, _cancelButton;
        [SerializeField] private TextMeshProUGUI  _buildingTypeText;

        public event Action<MiltaryBuildingType> OnBuyTurretPressed;
        public event Action OnCancelBuyPressed;

        private MiltaryBuildingType _miltaryBuildingType;
        public Action<GameObject> HasToClosePopup { get; set; }
        public Action PopupHasBeenClosed { get; set; }

        void Start()
        {
            _buyButton.onClick.AddListener(BuyTurretButtonPressed);
            _cancelButton.onClick.AddListener(CancelBuyButtonPressed);
        }

        private void CancelBuyButtonPressed()
        {
            HasToClosePopup.Invoke(gameObject);
            OnCancelBuyPressed?.Invoke();
        }

        private void BuyTurretButtonPressed()
        {
            HasToClosePopup.Invoke(gameObject);
            OnBuyTurretPressed?.Invoke(_miltaryBuildingType);
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(BuyTurretButtonPressed);
            _cancelButton.onClick.RemoveListener(CancelBuyButtonPressed);
        }

        public void SetData(MiltaryBuildingType miltaryBuildingType)
        {
            _buildingTypeText.SetText(miltaryBuildingType.ToString());
            _miltaryBuildingType = miltaryBuildingType;
        }

    }
}