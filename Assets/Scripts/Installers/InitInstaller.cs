using System.Linq;
using App.Models;
using App.SceneManagement;
using App.Services;
using Domain;
using Presentation.InputPlayer;
using Presentation.Managers;
using UnityEngine;
using Utils;

namespace Installers
{
    public class InitInstaller : MonoBehaviour
    {
        [SerializeField] private GameObject _readInputPlayerPrefab;
        [SerializeField] private GameObject _sceneChangerPrefab;
        [SerializeField] private GameObject _soundManagerPrefab;
        [SerializeField] private GameObject _timeManagerPrefab;
        [SerializeField] private GameObject _popupManagerPrefab;
        [SerializeField] private GameObject _constantsManagerPrefab;

        private GameObject _readInputPlayerInstance,
            _sceneChangerInstance,
            _soundManagerInstance,
            _popupManagerInstance,
            _constantsManagerInstance,
            _timeManagerInstance;

        private bool _initialized;

        public bool Initialized => _initialized;

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            var installersObject = FindObjectsOfType<InitInstaller>(true);
            if (installersObject.Any(x => x.Initialized)) return;

            _readInputPlayerInstance = Instantiate(_readInputPlayerPrefab);
            _sceneChangerInstance = Instantiate(_sceneChangerPrefab);
            _soundManagerInstance = Instantiate(_soundManagerPrefab);
            _timeManagerInstance = Instantiate(_timeManagerPrefab);
            _popupManagerInstance = Instantiate(_popupManagerPrefab);
            _constantsManagerInstance = Instantiate(_constantsManagerPrefab);

            //TODO HACERLO PARA N OBJETOS NO SOLO 1
            ServiceLocator.Instance.RegisterService<IJsonator>(new JsonUtililyTransformer());
            ServiceLocator.Instance.RegisterService<ISaver>(new SaveUsingPlayerPrefs());
            ServiceLocator.Instance.RegisterService<ILoader>(new LoadWithPlayerPrefs());
            ServiceLocator.Instance.RegisterService<GameDataService>(new GameDataService());
            ServiceLocator.Instance.RegisterService<SoundDataService>(new SoundDataService());

            ServiceLocator.Instance.RegisterService<SoundManager>(_soundManagerInstance.GetComponent<SoundManager>());
            ServiceLocator.Instance.RegisterService<ReadInputPlayer>(
                _readInputPlayerInstance.GetComponent<ReadInputPlayer>());
            ServiceLocator.Instance.RegisterService<SceneChanger>(_sceneChangerInstance.GetComponent<SceneChanger>());
            ServiceLocator.Instance.RegisterService<PopupManager>(_popupManagerInstance.GetComponent<PopupManager>());
            ServiceLocator.Instance.RegisterService<TimeManager>(_timeManagerInstance.GetComponent<TimeManager>());
            ServiceLocator.Instance.RegisterService(_constantsManagerInstance.GetComponent<ConstantsManager>());

            ServiceLocator.Instance.RegisterModel<IPlayerModel>(new PlayerModel());
            ServiceLocator.Instance.RegisterModel<ISceneModel>(new SceneModel());

            // DontDestroyOnLoad(this);
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_sceneChangerInstance);
            DontDestroyOnLoad(_readInputPlayerInstance);
            DontDestroyOnLoad(_soundManagerInstance);
            DontDestroyOnLoad(_popupManagerInstance);
            DontDestroyOnLoad(_timeManagerInstance);
            _initialized = true;
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