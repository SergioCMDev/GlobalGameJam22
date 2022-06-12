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
        private GameObject _currentopenedPopup;
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
            _currentopenedPopup = InstantiatePopup(popupType);
            return _currentopenedPopup.GetComponent<T>();
        }

        private GameObject InstantiatePopup(PopupType popupType)
        {
            GameObject prefab = _popupList.GetPrefabByType(popupType);
            _currentopenedPopup = Instantiate(prefab, transform, false);
            var closeablePopup = _currentopenedPopup.GetComponent<ICloseablePopup>();
            closeablePopup.HasToClosePopup += ClosePopup;
            _currentopenedPopup.gameObject.SetActive(false);
            _currentopenedPopup.GetComponentInChildren<Canvas>().worldCamera = _camera;
            return _currentopenedPopup;
        }

        private void ClosePopup(GameObject popup)
        {
            popup.SetActive(false);
            var closeablePopup = popup.GetComponent<ICloseablePopup>();
            closeablePopup.PopupHasBeenClosed.Invoke();
        }
    }
}