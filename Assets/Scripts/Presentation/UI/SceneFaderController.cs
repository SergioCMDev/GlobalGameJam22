﻿using System;
using App.Events;
using App.Models;
using Presentation.LoadingScene;
using Services.ScenesChanger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation.UI
{
    public class SceneFaderController : MonoBehaviour
    {
        private bool sceneLoaded;
        private bool fadedCompleted;
        [SerializeField] private CanvasFader _canvasFader;
        private ISceneModel _sceneModel;
        private SceneChangerService _sceneChangerService;
        private AsyncOperation operationLoadingScene;
        public FadeDisappearedEvent removedFadeEvent;

        void Start()
        {
            Debug.Log("Start SceneFaderController ");

            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
            Debug.Log("Start SceneFaderController 1");
        }

        public void GoToSelectedScene(ChangeToSpecificSceneEvent specificSceneEvent)
        {
            Debug.Log("GoToSelectedScene SceneFaderController 1");

            GoToSelectedScene(specificSceneEvent.SceneName);
        }

        private void GoToSelectedScene(string sceneToGo)
        {
            _sceneModel.PreviousScene = _sceneChangerService.GetCurrentSceneName();
            _sceneModel.NextScene = sceneToGo;
            _canvasFader.OnFadeCompleted += FadeCompleted;
            _canvasFader.ActivateFader();
            operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.NextScene, LoadSceneMode.Additive);
            operationLoadingScene.allowSceneActivation = false;
            operationLoadingScene.completed += SceneLoaded; //ONCE THE SCENE IS ALLOWED TO ACTIVATE
        }

        private void SceneLoaded(AsyncOperation obj)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneModel.NextScene));
            SceneManager.UnloadSceneAsync(_sceneModel.PreviousScene);
            RemoveFade();
        }

        private void SceneLoadedProgressDone()
        {
            Debug.Log("SceneLoadedProgressDone 1");

            sceneLoaded = true;
            InitializeNewScene();
        }

        private void FadeCompleted()
        {
            _canvasFader.OnFadeCompleted -= FadeCompleted;
            Debug.Log("FadeCompleted 1");

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
                SceneLoadedProgressDone();
            }

            Debug.Log("F333");
        }

        public void RemoveFade()
        {
            _canvasFader.OnReverseFadeCompleted += RemoveFadeEnds;
            _canvasFader.DeactivateFader();
            
        }
        private void RemoveFadeEnds()
        {
            _canvasFader.OnReverseFadeCompleted -= RemoveFadeEnds;
            removedFadeEvent.Fire();
        }

        public void HideImages()
        {
            _canvasFader.StatusImages(false);
        }
        public void ShowImages()
        {
            _canvasFader.StatusImages(true);
        }
    }
}