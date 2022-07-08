using System;
using App;
using Presentation.Interfaces;
using Presentation.Structs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    public class NeedMoreResourcesPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private Button _closeButton, _aceptButton;
        [SerializeField] private TextMeshProUGUI _title, _resourcesText;
        public Action<GameObject> HasToClosePopup { get; set; }
        public Action PopupHasBeenClosed { get; set; }

        void Start()
        {
            _closeButton.onClick.AddListener(ClosePopup);
            _aceptButton.onClick.AddListener(ClosePopup);
        }

        private void ClosePopup()
        {
            HasToClosePopup.Invoke(gameObject);
        }

        public void Init(ResourcesTuple resourcesNeeded, MilitaryBuildingType militaryBuildingType)
        {
            _title.SetText($"Need more resources for buying {militaryBuildingType}");
            _resourcesText.SetText(
                $"You need  {resourcesNeeded.Gold} Gold and {resourcesNeeded.Metal} metal to buy this building");
        }

        private void OnDisable()
        {
            _title.SetText(String.Empty);
            _resourcesText.SetText(String.Empty);
        }
    }
}