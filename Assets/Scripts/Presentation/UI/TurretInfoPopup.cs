using System;
using App;
using Services.Popups.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    public class TurretInfoPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Button _buyButton, _cancelButton;
        [SerializeField] private TextMeshProUGUI  _buildingTypeText;

        public event Action<MilitaryBuildingType> OnBuyTurretPressed;
        public event Action OnCancelBuyPressed;

        private MilitaryBuildingType _militaryBuildingType;
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
            OnBuyTurretPressed?.Invoke(_militaryBuildingType);
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(BuyTurretButtonPressed);
            _cancelButton.onClick.RemoveListener(CancelBuyButtonPressed);
        }

        public void SetData(MilitaryBuildingType militaryBuildingType)
        {
            _buildingTypeText.SetText(militaryBuildingType.ToString());
            _militaryBuildingType = militaryBuildingType;
        }

    }
}