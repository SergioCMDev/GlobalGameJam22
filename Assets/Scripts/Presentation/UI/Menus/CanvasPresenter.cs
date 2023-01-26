using System;
using System.Globalization;
using App.Buildings;
using App.Events;
using App.Resources;
using DG.Tweening;
using Services.ConstantsManager;
using Services.Popups;
using Services.ResourcesManager;
using Services.ScenesChanger;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Presentation.UI.Menus
{
    public class CanvasPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpText, _roundInformation;
        [SerializeField] private ChangeToNextSceneEvent _changeToNextSceneEvent;
        [SerializeField] private PlayerHasRestartedLevelEvent _playerHasRestartedLevelEvent;
        [SerializeField] private PlayerHasExitedLevelEvent _playerHasExitedLevelEvent;
        [SerializeField] private SetStatusDrawingTurretRangesEvent setStatusDrawingTurretRangesEvent;
        [SerializeField] private BuildingsSelectable _buildingsSelectable;
        [SerializeField] private SliderLogic sliderLogic;
        [SerializeField] private Button showRangeButton, pauseButton, skipRound;
        private Constants _constants;
        private bool _skipTimer, _timerIsRunning;

        private ResourcesManagerService _resourcesManager;
        private PopupGenerator _popupManager;

            //ONLY FOR Debugging
        [SerializeField]private GameStatusController gameStatusController;
        public event Action<MilitaryBuildingType> OnPlayerWantsToSetBuildingInGrid;
        public event Action OnSystemCancelsBuy;

        void Start()
        {
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManagerService>();
            _popupManager = ServiceLocator.Instance.GetService<PopupGenerator>();
            
            _constants = ServiceLocator.Instance.GetService<ConstantsManagerService>().Constants;
            SetInitialResources();
            _buildingsSelectable.OnPlayerWantsToBuyBuilding += AllowSetPositionOfTurret;
            pauseButton.onClick.AddListener(ShowPauseMenu);
            skipRound.onClick.AddListener(SkipRound);
            
            
        }

        private void SkipRound()
        {
            gameStatusController.SkipRound();
        }

        private void ShowPauseMenu()
        {
            StopGame();
            Time.timeScale = 0;
            var pausePopup = _popupManager.InstantiatePopup<PausePopup>(PopupGenerator.PopupType.Pause);
            pausePopup.gameObject.SetActive(true);
            pausePopup.OnQuitLevelClicked += GoToMainLevel;
            pausePopup.OnRestartLevelClicked += RestartLevel;
            pausePopup.OnResumeLevelClicked += ResumeLevel;
        }

        private void ResumeLevel(GameObject popup)
        {
            Time.timeScale = 1;
            sliderLogic.InitTimerLogic();
            popup.SetActive(false);
        }

        private void RestartLevel()
        {
            _playerHasRestartedLevelEvent.Fire();
        }
        
        //USED BY PointerDataEvent
        public void SetStatusDrawingTurretRanges(bool status)
        {
            setStatusDrawingTurretRangesEvent.drawingStatus = status;
            setStatusDrawingTurretRangesEvent.Fire();
        }

        private void GoToMainLevel()
        {
            _popupManager.ForceCloseCurrentOpenedPopup();
            _playerHasExitedLevelEvent.Fire();
        }

        private void RestartButtonPressedLevel()
        {
            _playerHasRestartedLevelEvent.Fire();
        }

        private void GoToNextLevel()
        {
            
            _changeToNextSceneEvent.Fire();
        }

        private void AllowSetPositionOfTurret(MilitaryBuildingType militaryBuildingType)
        {
            SetBuildingSelectableViewStatus(false);
            OnPlayerWantsToSetBuildingInGrid?.Invoke(militaryBuildingType);
            showRangeButton.gameObject.SetActive(false);
        }

        public void SetBuildingSelectableViewStatus(bool status)
        {
            SetShowRangeButtonStatus(status);
            _buildingsSelectable.gameObject.SetActive(status);
        }

        public void UpdateResources(UpdateUIResourcesEvent resourcesEvent)
        {
            UpdateResources(resourcesEvent.previousQuantity, resourcesEvent.currentQuantity);
        }

        private void UpdateResources(int resourcesEventPreviousQuantity, int resourcesEventCurrentQuantity)
        {
            var value = resourcesEventPreviousQuantity;
            DOTween.To(() => value, x => value = x, resourcesEventCurrentQuantity,
                    _constants.durationFloatIncrementTween)
                .OnUpdate(() => { _tmpText.SetText(value.ToString(CultureInfo.InvariantCulture)); });
        }

        private void SetInitialResources()
        {
            _tmpText.SetText($" {_resourcesManager.GetGold()}");
        }

        public void ShowWinMenu(ShowWinMenuUIEvent showWinMenuUIEvent)
        {
            SetBuildingSelectableViewStatus(false);

            var popupComponent =
                _popupManager.InstantiatePopup<PlayerHasWonPopup>(PopupGenerator.PopupType.PlayerHasWon);

            popupComponent.OnRestartButtonPressed += RestartButtonPressedLevel;
            popupComponent.OnGoToMainMenuButtonPressed += GoToMainLevel;
            popupComponent.OnContinueButtonPressed += GoToNextLevel;
            StopGame();
            popupComponent.gameObject.SetActive(true);
            SetShowRangeButtonStatus(false);
        }

        public void PlayerHasLost(ShowLostMenuUIEvent showLostMenuUIEvent)
        {
            SetBuildingSelectableViewStatus(false);
            var popupComponent =
                _popupManager.InstantiatePopup<PlayerHasLostPopup>(PopupGenerator.PopupType.PlayerHasLost);

            popupComponent.OnRestartButtonPressed += RestartButtonPressedLevel;
            popupComponent.OnGoToMainMenuButtonPressed += GoToMainLevel;
            popupComponent.gameObject.SetActive(true);
            SetShowRangeButtonStatus(false);
        }

        public void ShowNeedMoreResourcesPanel(ResourcesTuple resourcesNeeded,
            MilitaryBuildingType militaryBuildingType)
        {
            var popUpInstance =
                _popupManager.InstantiatePopup<NeedMoreResourcesPopup>(PopupGenerator.PopupType.NeedMoreResources);
            var popupComponent = popUpInstance.GetComponent<NeedMoreResourcesPopup>();
            popUpInstance.gameObject.SetActive(true);
            popupComponent.Init(resourcesNeeded, militaryBuildingType);
        }

        public void CancelPendingActivitiesOfPlayer()
        {
            OnSystemCancelsBuy?.Invoke();
            SetShowRangeButtonStatus(true);
        }

        public void UpdateRoundInformation(int currentRound, int numberOfRoundsPerLevel)
        {
            _roundInformation.SetText($"{currentRound}/{numberOfRoundsPerLevel}");
        }

        private void StopGame()
        {
            sliderLogic.StopTimerLogic();
        }

        public void SetShowRangeButtonStatus(bool status)
        {
            showRangeButton.gameObject.SetActive(status);
        }
    }
}