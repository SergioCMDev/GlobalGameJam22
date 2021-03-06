using System.Linq;
using App.Events;
using App.Models;
using App.SceneManagement;
using App.Services;
using Domain;
using Presentation;
using Presentation.InputPlayer;
using Presentation.Managers;
using UnityEngine;
using Utils;

namespace Installers
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameObject _readInputPlayerPrefab;
        [SerializeField] private GameObject _sceneChangerPrefab;
        [SerializeField] private GameObject _soundManagerPrefab;
        [SerializeField] private GameObject _timeManagerPrefab;
        [SerializeField] private GameObject _buildingManagerPrefab;
        [SerializeField] private GameObject _popupManagerPrefab;
        [SerializeField] private GameObject _constantsManagerPrefab;

        [SerializeField] private GameObject _resourcesManagerPrefab;

        private GameObject _readInputPlayerInstance,
            _sceneChangerInstance,
            _soundManagerInstance,
            _buildingManagerInstance,
            _resourcesManagerInstance,
            _popupManagerInstance,
            _constantsManagerInstance,
            _timeManagerInstance;

        private bool _initialized;

        private bool Initialized => _initialized;

        private void Awake()
        {
            var installersObject = FindObjectsOfType<GameInstaller>(true);
            if (installersObject.Any(x => x.Initialized)) return;

            var initInstallersObject = FindObjectsOfType<InitInstaller>(true);
            if (initInstallersObject.Any(x => x.Initialized))
            {
                InitGameLogic();
            }
            else
            {
                InitCommonLogic();
                InitGameLogic();
            }

            DontDestroyOnLoad(this);

            _initialized = true;
        }

        private void InitCommonLogic()
        {
            _readInputPlayerInstance = Instantiate(_readInputPlayerPrefab);
            _sceneChangerInstance = Instantiate(_sceneChangerPrefab);
            _soundManagerInstance = Instantiate(_soundManagerPrefab);
            _timeManagerInstance = Instantiate(_timeManagerPrefab);
            _popupManagerInstance = Instantiate(_popupManagerPrefab);
            _constantsManagerInstance = Instantiate(_constantsManagerPrefab);

            ServiceLocator.Instance.RegisterService(
                _soundManagerInstance.GetComponent<SoundManager>());
            ServiceLocator.Instance.RegisterService(
                _readInputPlayerInstance.GetComponent<ReadInputPlayer>());
            ServiceLocator.Instance.RegisterService(
                _sceneChangerInstance.GetComponent<SceneChanger>());
            ServiceLocator.Instance.RegisterService(
                _popupManagerInstance.GetComponent<PopupManager>());
            ServiceLocator.Instance.RegisterService(_timeManagerInstance.GetComponent<TimeManager>());
            ServiceLocator.Instance.RegisterService(_constantsManagerInstance.GetComponent<ConstantsManager>());


            //TODO HACERLO PARA N OBJETOS NO SOLO 1
            ServiceLocator.Instance.RegisterService<IJsonator>(new JsonUtililyTransformer());
            ServiceLocator.Instance.RegisterService<ISaver>(new SaveUsingPlayerPrefs());
            ServiceLocator.Instance.RegisterService<ILoader>(new LoadWithPlayerPrefs());
            ServiceLocator.Instance.RegisterService(new GameDataService());
            ServiceLocator.Instance.RegisterService(new SoundDataService());

            ServiceLocator.Instance.RegisterModel<IPlayerModel>(new PlayerModel());
            ServiceLocator.Instance.RegisterModel<ISceneModel>(new SceneModel());

            DontDestroyOnLoad(_sceneChangerInstance);
            DontDestroyOnLoad(_readInputPlayerInstance);
            DontDestroyOnLoad(_soundManagerInstance);
            DontDestroyOnLoad(_timeManagerInstance);
            DontDestroyOnLoad(_popupManagerInstance);
        }

        private void InitGameLogic()
        {
            _buildingManagerInstance = Instantiate(_buildingManagerPrefab);
            _resourcesManagerInstance = Instantiate(_resourcesManagerPrefab);
            ServiceLocator.Instance.RegisterService<ResourcesManager>(_resourcesManagerInstance
                .GetComponent<ResourcesManager>());
            ServiceLocator.Instance.RegisterService<MilitaryBuildingManager>(_buildingManagerInstance
                .GetComponent<MilitaryBuildingManager>());


            ServiceLocator.Instance.RegisterModel<IResourcesModel>(new ResourcesModel());
            ServiceLocator.Instance.RegisterModel<IBuildingStatusModel>(new BuildingStatusModel());

            DontDestroyOnLoad(_resourcesManagerInstance);
            DontDestroyOnLoad(_buildingManagerInstance);
        }

        private void OnDestroy()
        {
            // ServiceLocator.Instance.RegisterService<SoundManager>(_soundManagerInstance.GetComponent<SoundManager>());
            // ServiceLocator.Instance.UnregisterService<ReadInputPlayer>();
            // ServiceLocator.Instance.UnregisterService<SceneChanger>();
            // ServiceLocator.Instance.UnregisterService<TimeManager>();
            // ServiceLocator.Instance.UnregisterService<CollectibleManager>();
            // DontDestroyOnLoad(this);
        }
    }
}