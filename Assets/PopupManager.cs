using System;
using UnityEngine;

[Serializable]
public struct PopupGetter
{
    public PopupType PopupType;
    public GameObject Prefab;
}

public enum PopupType
{
    NeedMoreResources
}

public class PopupManager : MonoBehaviour
{
    // [SerializeField] private GameObject _prefab;
    [SerializeField] private PopupList _popupList;
    private GameObject _currentopenedPopup;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void InstantiatePopup(InstantiatePopupEvent instantiatePopupEvent)
    {
        GameObject prefab = _popupList.GetPrefabByType(instantiatePopupEvent.popupType);
        _currentopenedPopup = Instantiate(prefab, transform, false);
        var closeablePopup = _currentopenedPopup.GetComponent<ICloseablePopup>();
        closeablePopup.OnClosePopup += ClosePopup;
        _currentopenedPopup.GetComponentInChildren<Canvas>().worldCamera = _camera;
    }

    private void ClosePopup(GameObject popup)
    {
        popup.SetActive(false);
    }
}