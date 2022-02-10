using Application_;
using Application_.Events;
using Application_.SceneManagement;
using Presentation.Events;
using Presentation.Managers;
using TMPro;
using UnityEngine;
using Utils;

namespace Presentation.Menus
{
    public class CanvasPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmpText;
        [SerializeField] private PlayerWantsToBuyBuildingEvent _playerWantsToBuyBuildingEvent;
        [SerializeField] private ChangeToNextSceneEvent _changeToNextSceneEvent;
        [SerializeField] private ChangeToSpecificSceneEvent _changeToSpecificSceneEvent;
        [SerializeField] private PlayerHasRestartedLevelEvent _playerHasRestartedLevelEvent;
        [SerializeField] private BuildingsSelectable _buildingsSelectable;
        [SerializeField] private WinLoseMenuView _winLoseMenuView;
        private SceneChanger _sceneChanger;

        private ResourcesManager _resourcesManager;

        // Start is called before the first frame update
        void Start()
        {
            _resourcesManager = ServiceLocator.Instance.GetService<ResourcesManager>();
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
            UpdateResources(null);
            
            _buildingsSelectable.OnPlayerWantsToBuyBuilding += OnPlayerWantsToBuyBuilding;
            _winLoseMenuView.OnContinueButtonPressed += ContinueButtonPressedLevel;
            _winLoseMenuView.OnRestartButtonPressed += RestartButtonPressedLevel;
            _winLoseMenuView.OnGoToMainMenuButtonPressed += OnGoToMainLevel;
        }

        private void OnGoToMainLevel()
        {
            _changeToSpecificSceneEvent.SceneName = _sceneChanger.GetMainMenuSceneName();
            _changeToSpecificSceneEvent.Fire();
        }

        private void RestartButtonPressedLevel()
        {
            _playerHasRestartedLevelEvent.Fire();
        }
        
        private void ContinueButtonPressedLevel()
        {
            _changeToNextSceneEvent.Fire();
        }

        private void OnPlayerWantsToBuyBuilding(BuildingType buildingType)
        {
            _playerWantsToBuyBuildingEvent.BuildingType = buildingType;
            _playerWantsToBuyBuildingEvent.Fire();
        }

        public void UpdateResources(UpdateUIResourcesEvent resourcesEvent)
        {
            _tmpText.SetText($"Recurso {_resourcesManager.ResourcesModel.Gold}");
        }

        public void PlayerHasWon(ShowWinMenuUIEvent showWinMenuUIEvent)
        {
            _winLoseMenuView.ShowWinImage();
            _buildingsSelectable.gameObject.SetActive(false);
        }

        public void PlayerHasLost(ShowLostMenuUIEvent showLostMenuUIEvent)
        {
            _winLoseMenuView.ShowLoseImage();
            _buildingsSelectable.gameObject.SetActive(false);
        }
    }
}