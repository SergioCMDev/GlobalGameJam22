using System;
using Presentation.Events;
using Presentation.Interfaces;
using Presentation.UI;
using UnityEngine;

namespace Presentation.Managers
{
    [Serializable]
    public struct PopupGetter
    {
        public PopupType PopupType;
        public GameObject Prefab;
    }

    public enum PopupType
    {
        NeedMoreResources,
        PlayerHasLost,
        PlayerHasWon,
        NewRound,
        TurretInformation
    }

    public class PopupManager : MonoBehaviour
    {
        [SerializeField] private PopupList _popupList;
        private GameObject _currentOpenedPopup;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void InstantiatePopup(InstantiatePopupEvent instantiatePopupEvent)
        {
            InstantiatePopup(instantiatePopupEvent.popupType);
        }

        public T InstantiatePopup<T>(PopupType popupType)
        {
            _currentOpenedPopup = InstantiatePopup(popupType);
            return _currentOpenedPopup.GetComponent<T>();
        }

        private GameObject InstantiatePopup(PopupType popupType)
        {
            GameObject prefab = _popupList.GetPrefabByType(popupType);
            _currentOpenedPopup = Instantiate(prefab, transform, false);
            var closeablePopup = _currentOpenedPopup.GetComponent<ICloseablePopup>();
            closeablePopup.HasToClosePopup += ClosePopup;
            _currentOpenedPopup.gameObject.SetActive(false);
            _currentOpenedPopup.GetComponentInChildren<Canvas>().worldCamera = _camera;
            return _currentOpenedPopup;
        }

        private void ClosePopup(GameObject popup)
        {
            popup.SetActive(false);
            var closeablePopup = popup.GetComponent<ICloseablePopup>();
            closeablePopup.PopupHasBeenClosed.Invoke();
        }
        public void CloseCurrentOpenedPopup()
        {
            ClosePopup(_currentOpenedPopup);
        }
    }
}