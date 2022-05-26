using System.Linq;
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

        [SerializeField] private GameObject _resourcesManagerPrefab;

        private GameObject _readInputPlayerInstance,
            _sceneChangerInstance,
            _soundManagerInstance,
            _buildingManagerInstance,
            _resourcesManagerInstance,
            _popupManagerInstance,
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

            _buildingManagerInstance = Instantiate(_buildingManagerPrefab);
            _resourcesManagerInstance = Instantiate(_resourcesManagerPrefab);
            ServiceLocator.Instance.RegisterService<SoundManager>(
                _soundManagerInstance.GetComponent<SoundManager>());
            ServiceLocator.Instance.RegisterService<ReadInputPlayer>(
                _readInputPlayerInstance.GetComponent<ReadInputPlayer>());
            ServiceLocator.Instance.RegisterService<SceneChanger>(
                _sceneChangerInstance.GetComponent<SceneChanger>());
            ServiceLocator.Instance.RegisterService<PopupManager>(
                _popupManagerInstance.GetComponent<PopupManager>());
            ServiceLocator.Instance.RegisterService<TimeManager>(_timeManagerInstance.GetComponent<TimeManager>());


            //TODO HACERLO PARA N OBJETOS NO SOLO 1
            ServiceLocator.Instance.RegisterService<IJsonator>(new JsonUtililyTransformer());
            ServiceLocator.Instance.RegisterService<ISaver>(new SaveUsingPlayerPrefs());
            ServiceLocator.Instance.RegisterService<ILoader>(new LoadWithPlayerPrefs());
            ServiceLocator.Instance.RegisterService<GameDataService>(new GameDataService());
            ServiceLocator.Instance.RegisterService<SoundDataService>(new SoundDataService());
            
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
            ServiceLocator.Instance.RegisterService<BuildingManager>(_buildingManagerInstance
                .GetComponent<BuildingManager>());


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