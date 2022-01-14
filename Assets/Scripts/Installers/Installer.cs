﻿using System.Linq;
using Application_.Models;
using Application_.SceneManagement;
using Application_.Services;
using Domain;
using Presentation.Input;
using Presentation.Managers;
using UnityEngine;
using Utils;

namespace Presentation.Installers
{
    public class Installer : MonoBehaviour
    {
        [SerializeField] private GameObject _readInputPlayerPrefab;
        [SerializeField] private GameObject _sceneChangerPrefab;
        [SerializeField] private GameObject _soundManagerPrefab;
        [SerializeField] private GameObject _timeManagerPrefab;
        [SerializeField] private GameObject _collectibleManagerPrefab;

        private GameObject _readInputPlayerInstance,
            _sceneChangerInstance,
            _soundManagerInstance,
            _timeManagerInstance,
            _collectibleManagerInstance,
            _levelControllerInstance;

        private bool _initialized;

        private bool Initialized => _initialized;

        private void Awake()
        {
            var installersObject = FindObjectsOfType<Installer>(true);
            if (installersObject.Any(x => x.Initialized)) return;

            _readInputPlayerInstance = Instantiate(_readInputPlayerPrefab);
            _sceneChangerInstance = Instantiate(_sceneChangerPrefab);
            _soundManagerInstance = Instantiate(_soundManagerPrefab);
            _timeManagerInstance = Instantiate(_timeManagerPrefab);
            _collectibleManagerInstance = Instantiate(_collectibleManagerPrefab);


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
            ServiceLocator.Instance.RegisterService<TimeManager>(_timeManagerInstance.GetComponent<TimeManager>());
            ServiceLocator.Instance.RegisterService<CollectibleManager>(_collectibleManagerInstance
                .GetComponent<CollectibleManager>());
            ServiceLocator.Instance.RegisterModel<IPlayerModel>(new PlayerModel());
            ServiceLocator.Instance.RegisterModel<ISceneModel>(new SceneModel());

            ServiceLocator.Instance.RegisterService<CheckpointUpdaterService>(new CheckpointUpdaterService());
            // DontDestroyOnLoad(this);
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_sceneChangerInstance);
            DontDestroyOnLoad(_readInputPlayerInstance);
            DontDestroyOnLoad(_soundManagerInstance);
            DontDestroyOnLoad(_timeManagerInstance);
            DontDestroyOnLoad(_collectibleManagerInstance);
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