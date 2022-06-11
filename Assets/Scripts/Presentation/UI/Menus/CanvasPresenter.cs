using System;
using System.Collections.Generic;
using App;
using App.Events;
using App.SceneManagement;
using Presentation.Interfaces;
using Presentation.Managers;
using Presentation.Structs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace Presentation.UI.Menus
{
    public class CanvasPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpText;
        [SerializeField] private ChangeToNextSceneEvent _changeToNextSceneEvent;
        [SerializeField] private ChangeToSpecificSceneEvent _changeToSpecificSceneEvent;
        [SerializeField] private PlayerHasRestartedLevelEvent _playerHasRestartedLevelEvent;
        [SerializeField] private SetStatusDrawingTurretRangesEvent setStatusDrawingTurretRangesEvent;
        [SerializeField] private BuildingsSelectable _buildingsSelectable;
        [SerializeField] private SliderBarView _builderTimer, _defensiveTimer;
        private SceneChanger _sceneChanger;
        private bool _skipTimer, _timerIsRunning, _disabledBuildingView;

        private ResourcesManager _resourcesManager;
        private PopupManager _popupManager;
        private float _remainingTime;
        private SliderBarView _currentSliderBarView;
        public event Action<BuildingType> OnPlayerWantsToSetBuildingInGrid;

        void Start()
        {
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManager>();
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
            _popupManager = ServiceLocator.Instance.GetService<PopupManager>();
            UpdateResources();
            _buildingsSelectable.OnPlayerWantsToBuyBuilding += AllowSetPositionOfTurret;
        }
        
//USED BY PointerDataEvent
        public void SetStatusDrawingTurretRanges(bool status)
        {
            setStatusDrawingTurretRangesEvent.drawingStatus = status;
            setStatusDrawingTurretRangesEvent.Fire();
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
        //
        // private void PlayerWantsToBuyBuilding(BuildingType buildingType)
        // {
        //     AllowSetPositionOfTurret(buildingType);
        //     // var popUpInstance = _popupManager.InstantiatePopup(PopupType.TurretInformation);
        //     // var closeablePopup = popUpInstance.GetComponent<ICloseablePopup>();
        //     // var popupComponent = popUpInstance.GetComponent<TurretInfoPopup>();
        //     // closeablePopup.OnClosePopup += OnClosePopUp;
        //     //
        //     // popupComponent.gameObject.SetActive(true);
        //     // popupComponent.SetData(buildingType);
        //     // popupComponent.OnBuyTurretPressed += AllowSetPositionOfTurret;
        //     // popupComponent.OnCancelBuyPressed += CancelBuy;
        //     // popUpInstance.gameObject.SetActive(true);
        // }

        private void AllowSetPositionOfTurret(BuildingType buildingType)
        {
            SetBuildingSelectableViewStatus(false);
            OnPlayerWantsToSetBuildingInGrid?.Invoke(buildingType);
        }
        
        public void SetBuildingSelectableViewStatus(bool status)
        {
            _buildingsSelectable.gameObject.SetActive(!_disabledBuildingView && status);
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
            SetBuildingSelectableViewStatus(false);

            var popUpInstance = _popupManager.InstantiatePopup(PopupType.PlayerHasWon);
            var closeablePopup = popUpInstance.GetComponent<ICloseablePopup>();
            var popupComponent = popUpInstance.GetComponent<PlayerHasWonPopup>();
            closeablePopup.OnClosePopup += OnClosePopUp;

            popupComponent.OnRestartButtonPressed += RestartButtonPressedLevel;
            popupComponent.OnGoToMainMenuButtonPressed += GoToMainLevel;
            popupComponent.OnContinueButtonPressed += GoToNextLevel;

            popUpInstance.gameObject.SetActive(true);
        }
        
        public void PlayerHasLost(ShowLostMenuUIEvent showLostMenuUIEvent)
        {
            SetBuildingSelectableViewStatus(false);
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

        public void SetBuilderTimerInitialValue(float time, Action onTimerHasEnded)
        {
            _builderTimer.SetMaxValue(time);

            _remainingTime = time;
            _currentSliderBarView = _builderTimer;
            _currentSliderBarView.OnSliderReachZero += () => onTimerHasEnded?.Invoke();

        }
        
        public void SetDefensiveTimerInitialValue(float time, Action onTimerHasEnded)
        {
            _defensiveTimer.SetMaxValue(time);
            
            _remainingTime = time;
            _currentSliderBarView = _defensiveTimer;
            _currentSliderBarView.OnSliderReachZero += () => onTimerHasEnded?.Invoke();

        }

        private void Update()
        {
            if (_skipTimer) return;
            _remainingTime -= Time.deltaTime;
            _currentSliderBarView.SetValue(_remainingTime);
        }

        public void InitTimerLogic(bool skipTimer)
        {
            _skipTimer = skipTimer;
        }
    }
}