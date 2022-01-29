using System.Linq;
using Application_.SceneManagement;
using Application_.Services;
using Domain;
using Presentation.Managers;
using UnityEngine;
using Utils;

namespace Installers
{
    public class InstallerMainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _soundManagerPrefab;
        [SerializeField] private GameObject _sceneChangerPrefab;
        private bool _initialized;

        private bool Initialized => _initialized;
        private GameObject _sceneChangerInstance, _soundManagerInstance;
        void Awake()
        {
            var installersObject = FindObjectsOfType<InstallerMainMenu>(true);
            if (installersObject.Any(x => x.Initialized)) return;
            _soundManagerInstance = Instantiate(_soundManagerPrefab);
            _sceneChangerInstance = Instantiate(_sceneChangerPrefab);

            ServiceLocator.Instance.RegisterService<IJsonator>(new JsonUtililyTransformer());
            ServiceLocator.Instance.RegisterService<ISaver>(new SaveUsingPlayerPrefs());
            ServiceLocator.Instance.RegisterService<ILoader>(new LoadWithPlayerPrefs());
        
            ServiceLocator.Instance.RegisterService<SceneChanger>(_sceneChangerInstance.GetComponent<SceneChanger>());
            ServiceLocator.Instance.RegisterService<SoundManager>(_soundManagerInstance.GetComponent<SoundManager>());
            ServiceLocator.Instance.RegisterService<GameDataService>(new GameDataService());
            ServiceLocator.Instance.RegisterService<SoundDataService>(new SoundDataService());
        }
    }
}
