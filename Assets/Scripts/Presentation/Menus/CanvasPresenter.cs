using System;
using App;
using App.Events;
using App.SceneManagement;
using Presentation.Managers;
using Presentation.Structs;
using TMPro;
using UnityEngine;
using Utils;

namespace Presentation.Menus
{
    public class CanvasPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpText;
        [SerializeField] private ChangeToNextSceneEvent _changeToNextSceneEvent;
        [SerializeField] private ChangeToSpecificSceneEvent _changeToSpecificSceneEvent;
        [SerializeField] private PlayerHasRestartedLevelEvent _playerHasRestartedLevelEvent;
        [SerializeField] private BuildingsSelectable _buildingsSelectable;
        [SerializeField] private WinMenuView _winMenuView;
        [SerializeField] private LoseMenuView _loseMenuView;
        [SerializeField] private TurretInfoMenuView _turretInfoMenuView;
        [SerializeField] private NeedMoreResourcesView _needMoreResourcesView;
        private SceneChanger _sceneChanger;

        private ResourcesManager _resourcesManager;

        public event Action<BuildingType> OnPlayerWantsToSetBuildingInGrid;

        void Start()
        {
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManager>();
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
            UpdateResources();
            _turretInfoMenuView.gameObject.SetActive(false);
            _loseMenuView.gameObject.SetActive(false);
            _winMenuView.gameObject.SetActive(false);
            _needMoreResourcesView.gameObject.SetActive(false);

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
            _turretInfoMenuView.gameObject.SetActive(true);
            _turretInfoMenuView.SetData(buildingType);
            _turretInfoMenuView.OnBuyTurretPressed += AllowSetPositionOfTurret;
            _turretInfoMenuView.OnCancelBuyPressed += CancelBuy;
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
            _turretInfoMenuView.gameObject.SetActive(false);
            _turretInfoMenuView.OnBuyTurretPressed -= AllowSetPositionOfTurret;
            _turretInfoMenuView.OnCancelBuyPressed -= CancelBuy;
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
            _winMenuView.gameObject.SetActive(true);
            _winMenuView.OnRestartButtonPressed += RestartButtonPressedLevel;
            _winMenuView.OnGoToMainMenuButtonPressed += GoToMainLevel;
            _winMenuView.OnContinueButtonPressed += GoToNextLevel;
        }

        private void CloseMenus()
        {
            _buildingsSelectable.gameObject.SetActive(false);
            _turretInfoMenuView.gameObject.SetActive(false);
            _needMoreResourcesView.gameObject.SetActive(false);
        }

        public void PlayerHasLost(ShowLostMenuUIEvent showLostMenuUIEvent)
        {
            _loseMenuView.gameObject.SetActive(true);
            CloseMenus();
            _loseMenuView.OnRestartButtonPressed += RestartButtonPressedLevel;
            _loseMenuView.OnGoToMainMenuButtonPressed += GoToMainLevel;
        }

        private void OnDestroy()
        {
            if (_loseMenuView)
            {
                _loseMenuView.OnRestartButtonPressed += RestartButtonPressedLevel;
                _loseMenuView.OnGoToMainMenuButtonPressed += GoToMainLevel;
            }

            if (!_winMenuView) return;
            _winMenuView.OnRestartButtonPressed += RestartButtonPressedLevel;
            _winMenuView.OnGoToMainMenuButtonPressed += GoToMainLevel;
            _winMenuView.OnContinueButtonPressed += GoToNextLevel;
        }

        public void ShowNeedMoreResourcesPanel(ResourcesTuple resourcesNeeded, BuildingType buildingType)
        {
            // _needMoreResourcesView.gameObject.SetActive(true);
            // _needMoreResourcesView.Init(resourcesNeeded, buildingType);
            // _needMoreResourcesView.OnClosePopup += () => { _needMoreResourcesView.gameObject.SetActive(false); };
        }
    }
}