using System;
using System.Collections;
using System.Globalization;
using App;
using App.Events;
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
        [SerializeField] private ChangeToSpecificSceneEvent _changeToSpecificSceneEvent;
        [SerializeField] private PlayerHasRestartedLevelEvent _playerHasRestartedLevelEvent;
        [SerializeField] private SetStatusDrawingTurretRangesEvent setStatusDrawingTurretRangesEvent;
        [SerializeField] private BuildingsSelectable _buildingsSelectable;
        [SerializeField] private SliderBarView _builderTimer, _defensiveTimer;
        [SerializeField] private Button _showRangeButton;
        private SceneChangerService _sceneChangerService;
        private Constants _constants;
        private bool _skipTimer, _timerIsRunning;

        private ResourcesManagerService _resourcesManagerOld;
        private PopupGenerator _popupManager;
        private float _remainingTime;
        private SliderBarView _currentSliderBarView;
        private bool _stopSlider;
        public event Action<MilitaryBuildingType> OnPlayerWantsToSetBuildingInGrid;
        public event Action OnSystemCancelsBuy;

        void Start()
        {
            _resourcesManagerOld = ServiceLocator.Instance.GetService<ResourcesManagerService>();
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            _popupManager = ServiceLocator.Instance.GetService<PopupGenerator>();
            _constants = ServiceLocator.Instance.GetService<ConstantsManagerService>().Constants;
            SetInitialResources();
            _buildingsSelectable.OnPlayerWantsToBuyBuilding += AllowSetPositionOfTurret;

            _stopSlider = false;
        }


        //USED BY PointerDataEvent
        public void SetStatusDrawingTurretRanges(bool status)
        {
            setStatusDrawingTurretRangesEvent.drawingStatus = status;
            setStatusDrawingTurretRangesEvent.Fire();
        }

        private void GoToMainLevel()
        {
            _changeToSpecificSceneEvent.SceneName = _sceneChangerService.GetMainMenuSceneName();
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

        private void AllowSetPositionOfTurret(MilitaryBuildingType militaryBuildingType)
        {
            SetBuildingSelectableViewStatus(false);
            OnPlayerWantsToSetBuildingInGrid?.Invoke(militaryBuildingType);
            _showRangeButton.gameObject.SetActive(false);
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
            DOTween.To(() => value, x => value = x, resourcesEventCurrentQuantity, _constants.durationFloatIncrementTween)
                .OnUpdate(() => { _tmpText.SetText(value.ToString(CultureInfo.InvariantCulture)); });
        }

        private void SetInitialResources()
        {
            _tmpText.SetText($" {_resourcesManagerOld.GetGold()}");
        }
        public void ShowWinMenu(ShowWinMenuUIEvent showWinMenuUIEvent)
        {
            SetBuildingSelectableViewStatus(false);

            var popupComponent = _popupManager.InstantiatePopup<PlayerHasWonPopup>(PopupGenerator.PopupType.PlayerHasWon);
            
            popupComponent.OnRestartButtonPressed += RestartButtonPressedLevel;
            popupComponent.OnGoToMainMenuButtonPressed += GoToMainLevel;
            popupComponent.OnContinueButtonPressed += GoToNextLevel;
            StopsTimerLogic();
            popupComponent.gameObject.SetActive(true);
            SetShowRangeButtonStatus(false);
        }

        public void PlayerHasLost(ShowLostMenuUIEvent showLostMenuUIEvent)
        {
            SetBuildingSelectableViewStatus(false);
            var popupComponent = _popupManager.InstantiatePopup<PlayerHasLostPopup>(PopupGenerator.PopupType.PlayerHasLost);
            
            popupComponent.OnRestartButtonPressed += RestartButtonPressedLevel;
            popupComponent.OnGoToMainMenuButtonPressed += GoToMainLevel;
            popupComponent.gameObject.SetActive(true);
            SetShowRangeButtonStatus(false);
        }

        public void ShowNeedMoreResourcesPanel(ResourcesTuple resourcesNeeded, MilitaryBuildingType militaryBuildingType)
        {
            var popUpInstance = _popupManager.InstantiatePopup<NeedMoreResourcesPopup>(PopupGenerator.PopupType.NeedMoreResources);
            var popupComponent = popUpInstance.GetComponent<NeedMoreResourcesPopup>();
            popUpInstance.gameObject.SetActive(true);
            popupComponent.Init(resourcesNeeded, militaryBuildingType);
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

        private IEnumerator StartSliderTimer()
        {
            do
            {
                _remainingTime -= Time.deltaTime;
                _currentSliderBarView.SetValue(_remainingTime);
                yield return null;
            } while (_remainingTime > 0 & !_stopSlider);
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

        public void InitTimerLogic()
        {
            _currentSliderBarView.OnSliderReachZero += StopTimerLogic;
            StartCoroutine(StartSliderTimer());
        }
        
        public void StopsTimerLogic()
        {
            _currentSliderBarView.OnSliderReachZero -= StopTimerLogic;
            _currentSliderBarView.enabled = false;
            _stopSlider = true;
            StopCoroutine(StartSliderTimer());
        }

        private void StopTimerLogic()
        {
            _currentSliderBarView.OnSliderReachZero -= StopTimerLogic;
        }

        public void SetShowRangeButtonStatus(bool status)
        {
            _showRangeButton.gameObject.SetActive(status);
        }
    }
}