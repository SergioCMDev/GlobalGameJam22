using System;
using UnityEngine;

namespace Presentation
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
        TurretInformation
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
            GameObject instance = InstantiatePopup(instantiatePopupEvent.popupType);
            var closeablePopup = instance.GetComponentInChildren<ICloseablePopup>();
            closeablePopup.OnClosePopup += ClosePopup;
            instance.gameObject.SetActive(true);
        }

        public GameObject InstantiatePopup(PopupType popupType)
        {
            GameObject prefab = _popupList.GetPrefabByType(popupType);
            _currentopenedPopup = Instantiate(prefab, transform, false);
            _currentopenedPopup.gameObject.SetActive(false);
            _currentopenedPopup.GetComponentInChildren<Canvas>().worldCamera = _camera;
            return _currentopenedPopup;
        }

        private void ClosePopup(GameObject popup)
        {
            popup.SetActive(false);
        }
    }
}