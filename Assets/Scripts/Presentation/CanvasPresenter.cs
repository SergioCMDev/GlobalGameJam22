using Application_;
using Application_.Events;
using Application_.SceneManagement;
using Presentation;
using Presentation.Managers;
using TMPro;
using UnityEngine;
using Utils;

namespace Presentation
{
    public class CanvasPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private PlayerWantsToBuyBuildingEvent _event;
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
            _sceneChanger.GoToMenu();
        }

        private void RestartButtonPressedLevel()
        {
            _sceneChanger.RestartScene(null);
        }
        
        private void ContinueButtonPressedLevel()
        {
            _sceneChanger.GoToNextScene();
        }

        private void OnPlayerWantsToBuyBuilding(BuildingType buildingType)
        {
            _event.BuildingType = buildingType;
            _event.Fire();
        }


        public void UpdateResources(UpdateUIResourcesEvent resourcesEvent)
        {
            _tmpText.SetText($"Recurso {_resourcesManager.ResourcesModel.Gold}");
        }


        public void PlayerHasWon(PlayerHasWonEvent playerHasWonEvent)
        {
            _winLoseMenuView.ShowWinImage();
            _buildingsSelectable.gameObject.SetActive(false);
            Debug.Log("HAS GANAO");
        }

        public void PlayerHasLost(PlayerHasLostEvent playerHasLostEvent)
        {
            _winLoseMenuView.ShowLoseImage();
            _buildingsSelectable.gameObject.SetActive(false);
            Debug.Log("HAS PERDIO");
        }
    }
}