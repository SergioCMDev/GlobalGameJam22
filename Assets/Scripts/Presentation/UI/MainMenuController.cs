using System;
using App.Events;
using App.Models;
using App.SceneManagement;
using Presentation.LoadingScene;
using Presentation.UI.Menus;
using Services.Popups;
using Services.ScenesChanger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private PlayMusicEvent _playMusicEvent;
        [SerializeField] private MusicSoundName _musicSoundName;
        [SerializeField] private CanvasFader _canvasFader;
        [SerializeField] private CanvasInitScenePresenter _canvasInitScenePresenter;
        private SceneChangerService _sceneChangerService;
        private ISceneModel _sceneModel;
        private AsyncOperation operationLoadingScene;
        private bool sceneLoaded;
        private bool fadedCompleted;

        void Start()
        {
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
            _canvasInitScenePresenter.OnStartNewGame += StartNewGame;
            _canvasInitScenePresenter.OnGoToSelectedScene += GoToSelectedScene;
            _playMusicEvent.soundName = _musicSoundName;
            _playMusicEvent.Fire();
        }

        private void StartNewGame()
        {
            var sceneName = _sceneChangerService.GetFirstSceneName();
            GoToSelectedScene(sceneName);
        }

        private void GoToSelectedScene(string sceneToGo)
        {
            _sceneModel.PreviousScene = _sceneChangerService.GetCurrentSceneName();
            _sceneModel.NextScene = sceneToGo;
            _canvasFader.OnFadeCompleted += FadeCompleted;
            _canvasFader.ActivateFader();
            operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.NextScene, LoadSceneMode.Additive);
            operationLoadingScene.allowSceneActivation = false;
            operationLoadingScene.completed += SceneLoaded1;   //ONCE THE SCENE IS ALLOWED TO ACTIVATE
        }

        private void SceneLoaded1(AsyncOperation obj)
        {
        }

        private void SceneLoaded()
        {
            sceneLoaded = true;
            InitializeNewScene();
        }
        
        // private void SceneLoaded(AsyncOperation obj)
        // {
        //     operationLoadingScene.completed -= SceneLoaded;
        //
        //     sceneLoaded = true;
        //     InitializeNewScene();
        // }


        private void FadeCompleted()
        {
            _canvasFader.OnFadeCompleted -= FadeCompleted;

            fadedCompleted = true;
            InitializeNewScene();
        }

        private void InitializeNewScene()
        {
            if (sceneLoaded && fadedCompleted)
                operationLoadingScene.allowSceneActivation = true;
        }

        public void Update()
        {
            if (sceneLoaded || operationLoadingScene == null) return;
            var progress = Mathf.Clamp01(operationLoadingScene.progress / 0.9f) * 100f;
            Debug.Log($"Progress {progress}");
            if (progress == 100)
            {
                SceneLoaded();
            }
            Debug.Log("F333");
        }
    }
}