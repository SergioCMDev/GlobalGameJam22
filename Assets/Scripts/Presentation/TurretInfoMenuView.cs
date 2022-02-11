using System;
using Application_;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public class TurretInfoMenuView : MonoBehaviour
    {
        [SerializeField] private Button _buyButton, _cancelButton;
        [SerializeField] private TextMeshProUGUI  _buildingTypeText;

        public event Action<BuildingType> OnBuyTurretPressed;
        public event Action OnCancelBuyPressed;

        private BuildingType _buildingType;

        void Start()
        {
            _buyButton.onClick.AddListener(BuyTurretButtonPressed);
            _cancelButton.onClick.AddListener(CancelBuyButtonPressed);
        }

        private void CancelBuyButtonPressed()
        {
            OnCancelBuyPressed.Invoke();
        }

        private void BuyTurretButtonPressed()
        {
            OnBuyTurretPressed.Invoke(_buildingType);
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(BuyTurretButtonPressed);
            _cancelButton.onClick.RemoveListener(CancelBuyButtonPressed);
        }

        public void SetData(BuildingType buildingType)
        {
            _buildingTypeText.SetText(buildingType.ToString());
            _buildingType = buildingType;
        }
    }
}