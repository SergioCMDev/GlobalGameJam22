using System;
using Services.Popups.Events;
using Services.Popups.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Popups
{
    [CreateAssetMenu(fileName = "PopupManager",
        menuName = "Loadable/Services/PopupManager")]
    public class PopupGenerator : LoadableComponent
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
            Pause,
            TurretInformation,
            Empty
        }

        [SerializeField] private PopupList popupList;
        private GameObject _currentOpenedPopup;
        private PopupType _currentlyOpenedType;
        private Camera _camera;
        private int _currentSortingOrder;
        private Transform _positionWhereSpawn;
        

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
            GameObject prefab = popupList.GetPrefabByType(popupType);

            _currentOpenedPopup = Instantiate(prefab, _positionWhereSpawn, false);
            var closeablePopup = _currentOpenedPopup.GetComponent<ICloseablePopup>();
            closeablePopup.HasToClosePopup += ClosePopup;
            _currentOpenedPopup.gameObject.SetActive(false);
            var canvas = _currentOpenedPopup.GetComponentInChildren<Canvas>();
            if (canvas.sortingOrder <= _currentSortingOrder)
            {
                canvas.sortingOrder = _currentSortingOrder + 1;
            }

            canvas.worldCamera = Camera.main;
            _currentSortingOrder = canvas.sortingOrder;
            _currentlyOpenedType = popupType;
            return _currentOpenedPopup;
        }


        private void ClosePopup(GameObject popup)
        {
            popup.SetActive(false);
            _currentlyOpenedType = PopupType.Empty;
            var closeablePopup = popup.GetComponent<ICloseablePopup>();
            closeablePopup.PopupHasBeenClosed?.Invoke();
            _currentSortingOrder--;

            Destroy(popup);
        }


        public void ForceCloseCurrentOpenedPopup()
        {
            var closeablePopup = _currentOpenedPopup.GetComponent<ICloseablePopup>();
            closeablePopup.PopupHasBeenClosed = null;
            ClosePopup(_currentOpenedPopup);
        }

        public bool IsCurrentlyOpened(PopupType popupType)
        {
            return _currentOpenedPopup != null && _currentlyOpenedType == popupType;
        }

        public void UpdateCamera()
        {
            var cameraToSelect = Camera.main;
            _camera = cameraToSelect;

        }

        public override void Execute()
        {
            // foreach (var camera in Camera.allCameras)
            // {
            //     SceneManager.GetActiveScene().
            //     if (camera.scene.name.Contains("Fader"))
            //     {
            //         continue;
            //     }
            //
            //     cameraToSelect = camera;
            // }

            UpdateCamera();
            _positionWhereSpawn = new GameObject().transform;
            _positionWhereSpawn.name = "PopupsContainer";
        }
    }
}