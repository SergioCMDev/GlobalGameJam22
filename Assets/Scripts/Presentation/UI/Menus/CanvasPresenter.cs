using System;
using App;
using App.Events;
using App.SceneManagement;
using Presentation.Interfaces;
using Presentation.Managers;
using Presentation.Structs;
using TMPro;
using UnityEngine;
using Utils;

namespace Presentation.UI.Menus
{
    public class CanvasPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpText;
        [SerializeField] private ChangeToNextSceneEvent _changeToNextSceneEvent;
        [SerializeField] private ChangeToSpecificSceneEvent _changeToSpecificSceneEvent;
        [SerializeField] private PlayerHasRestartedLevelEvent _playerHasRestartedLevelEvent;
        [SerializeField] private BuildingsSelectable _buildingsSelectable;
        private SceneChanger _sceneChanger;

        private ResourcesManager _resourcesManager;
        private PopupManager _popupManager;

        public event Action<BuildingType> OnPlayerWantsToSetBuildingInGrid;

        void Start()
        {
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManager>();
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
            _popupManager = ServiceLocator.Instance.GetService<PopupManager>();
            UpdateResources();
            

            _buildingsSelectable.OnPlayerWantsToBuyBuilding += PlayerWantsToBuyBuilding;
        }

        private void GoToMainLevel()
        {
            _changeToSpecificSceneEvent.SceneName = _sceneChanger.GetMainMenuSceneName();
            _changeToSpecificSceneEvent.Fire();
        }

        private void RestartButtonPressedLevel()
        {
            _playerHasRestartedLevelEvent.Fire();
        }

        private void GoToNextLevel()
        {
            _changeToNextSceneEvent.Fire();
        }

        private void PlayerWantsToBuyBuilding(BuildingType buildingType)
        {
            var popUpInstance = _popupManager.InstantiatePopup(PopupType.TurretInformation);
            var closeablePopup = popUpInstance.GetComponent<ICloseablePopup>();
            var popupComponent = popUpInstance.GetComponent<TurretInfoPopup>();
            closeablePopup.OnClosePopup += OnClosePopUp;
            
            popupComponent.gameObject.SetActive(true);
            popupComponent.SetData(buildingType);
            popupComponent.OnBuyTurretPressed += AllowSetPositionOfTurret;
            popupComponent.OnCancelBuyPressed += CancelBuy;
            popUpInstance.gameObject.SetActive(true);
        }

        private void AllowSetPositionOfTurret(BuildingType buildingType)
        {
            HideTurretInfoView();
            SetBuildingSelectableStatus(false);
            OnPlayerWantsToSetBuildingInGrid.Invoke(buildingType);
        }

        private void CancelBuy()
        {
            HideTurretInfoView();
        }

        public void SetBuildingSelectableStatus(bool status)
        {
            _buildingsSelectable.gameObject.SetActive(status);
        }

        private void HideTurretInfoView()
        {
            // turretInfoPopup.gameObject.SetActive(false);
            // turretInfoPopup.OnBuyTurretPressed -= AllowSetPositionOfTurret;
            // turretInfoPopup.OnCancelBuyPressed -= CancelBuy;
        }

        public void UpdateResources(UpdateUIResourcesEvent resourcesEvent)
        {
            UpdateResources();
        }

        private void UpdateResources()
        {
            _tmpText.SetText($"Recurso {_resourcesManager.ResourcesModel.Gold}");
        }

        public void PlayerHasWon(ShowWinMenuUIEvent showWinMenuUIEvent)
        {
            CloseMenus();
            
            var popUpInstance = _popupManager.InstantiatePopup(PopupType.PlayerHasWon);
            var closeablePopup = popUpInstance.GetComponent<ICloseablePopup>();
            var popupComponent = popUpInstance.GetComponent<PlayerHasWonPopup>();
            closeablePopup.OnClosePopup += OnClosePopUp;
            
            popupComponent.OnRestartButtonPressed += RestartButtonPressedLevel;
            popupComponent.OnGoToMainMenuButtonPressed += GoToMainLevel;
            popupComponent.OnContinueButtonPressed += GoToNextLevel;

            popUpInstance.gameObject.SetActive(true);
        }

        private void CloseMenus()
        {
            _buildingsSelectable.gameObject.SetActive(false);
        }

        public void PlayerHasLost(ShowLostMenuUIEvent showLostMenuUIEvent)
        {
            CloseMenus();
            var popUpInstance = _popupManager.InstantiatePopup(PopupType.PlayerHasLost);
            var closeablePopup = popUpInstance.GetComponent<ICloseablePopup>();
            var popupComponent = popUpInstance.GetComponent<PlayerHasLostPopup>();
            popUpInstance.gameObject.SetActive(true);
            closeablePopup.OnClosePopup += OnClosePopUp;
            
            popupComponent.OnRestartButtonPressed += RestartButtonPressedLevel;
            popupComponent.OnGoToMainMenuButtonPressed += GoToMainLevel;
        }

       
        public void ShowNeedMoreResourcesPanel(ResourcesTuple resourcesNeeded, BuildingType buildingType)
        {
            var popUpInstance = _popupManager.InstantiatePopup(PopupType.NeedMoreResources);
            var closeablePopup = popUpInstance.GetComponent<ICloseablePopup>();
            var popupComponent = popUpInstance.GetComponent<NeedMoreResourcesPopup>();
            popUpInstance.gameObject.SetActive(true);
            closeablePopup.OnClosePopup += OnClosePopUp;
            popupComponent.Init(resourcesNeeded, buildingType);
        }

        private void OnClosePopUp(GameObject obj)
        {
            obj.gameObject.SetActive(false);
        }
    }
}