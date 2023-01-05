using App.Models;
using Domain;
using Presentation.InputPlayer;
using Presentation.Managers;
using Services.Popups;
using UnityEngine;
using Utils;

namespace Installers
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameObject _readInputPlayerPrefab;
        [SerializeField] private GameObject _soundManagerPrefab;
        // [SerializeField] private GameObject _timeManagerPrefab;
        // [SerializeField] private GameObject _buildingManagerPrefab;


        private GameObject _readInputPlayerInstance,
            // _sceneChangerInstance,
            _soundManagerInstance
            // _buildingManagerInstance,
            // _timeManagerInstance
            ;

        private bool _initialized;

        private bool Initialized => _initialized;

        // private void Awake()
        // {
        //     var installersObject = FindObjectsOfType<GameInstaller>(true);
        //     if (installersObject.Any(x => x.Initialized)) return;
        //
        //     var initInstallersObject = FindObjectsOfType<InitInstaller>(true);
        //     if (initInstallersObject.Any(x => x.Initialized))
        //     {
        //         InitGameLogic();
        //     }
        //     else
        //     {
        //         InitCommonLogic();
        //         InitGameLogic();
        //     }
        //
        //     DontDestroyOnLoad(this);
        //
        //     _initialized = true;
        // }

        private void InitCommonLogic()
        {
            _readInputPlayerInstance = Instantiate(_readInputPlayerPrefab);
            _soundManagerInstance = Instantiate(_soundManagerPrefab);
            // _timeManagerInstance = Instantiate(_timeManagerPrefab);

            ServiceLocator.Instance.RegisterService(
                _soundManagerInstance.GetComponent<SoundPlayer>());
            ServiceLocator.Instance.RegisterService(
                _readInputPlayerInstance.GetComponent<ReadInputPlayer>());
            // ServiceLocator.Instance.RegisterService(
                // _sceneChangerInstance.GetComponent<SceneChangerService>());



            //TODO HACERLO PARA N OBJETOS NO SOLO 1
            // ServiceLocator.Instance.RegisterService<IJsonator>(new JsonUtililyTransformer());
            // ServiceLocator.Instance.RegisterService<ISaver>(new SaveUsingPlayerPrefs());
            // ServiceLocator.Instance.RegisterService<ILoader>(new LoadWithPlayerPrefs());
            // ServiceLocator.Instance.RegisterService(new GameDataService());
            // ServiceLocator.Instance.RegisterService(new SoundDataService());

            // ServiceLocator.Instance.RegisterModel<IPlayerModel>(new PlayerModel());
            // ServiceLocator.Instance.RegisterModel<ISceneModel>(new SceneModel());

            // DontDestroyOnLoad(_sceneChangerInstance);
            DontDestroyOnLoad(_readInputPlayerInstance);
            DontDestroyOnLoad(_soundManagerInstance);
            // DontDestroyOnLoad(_timeManagerInstance);
        }

        private void InitGameLogic()
        {
            // _buildingManagerInstance = Instantiate(_buildingManagerPrefab);
          
            // ServiceLocator.Instance.RegisterService<MilitaryBuildingManager>(_buildingManagerInstance
            //     .GetComponent<MilitaryBuildingManager>());

            // ServiceLocator.Instance.RegisterModel<IResourcesModel>(new ResourcesModel());
            // ServiceLocator.Instance.RegisterModel<IBuildingStatusModel>(new BuildingStatusModel());

            // DontDestroyOnLoad(_buildingManagerInstance);
        }
        
    }
}