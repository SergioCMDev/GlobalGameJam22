using System;
using App;
using Presentation.Structs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedMoreResourcesView : MonoBehaviour
{
    [SerializeField] private Button _closeButton, _aceptButton;
    [SerializeField] private TextMeshProUGUI _title, _resourcesText;
    public Action OnClosePopup;

    void Start()
    {
        _closeButton.onClick.AddListener(ClosePopup);
        _aceptButton.onClick.AddListener(ClosePopup);
    }

    private void ClosePopup()
    {
        OnClosePopup.Invoke();
    }

    public void Init(ResourcesTuple resourcesNeeded, BuildingType buildingType)
    {
        _title.SetText($"Need more resources for buying {buildingType}");
        _resourcesText.SetText(
            $"You need  {resourcesNeeded.Gold} Gold and {resourcesNeeded.Metal} metal to buy this building");
    }

    private void OnDisable()
    {
        _title.SetText(String.Empty);
        _resourcesText.SetText(String.Empty);
    }
}