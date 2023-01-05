using App.Events;
using App.Models;
using App.SceneManagement;
using Presentation.LoadingScene;
using Services.Popups;
using Services.ScenesChanger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation.UI
{
    public class FaderController : MonoBehaviour
    {
    
        [SerializeField] private CanvasFader _canvasFader;
        private SceneChangerService _sceneChangerService;
        private AsyncOperation operationLoadingScene;
        private ISceneModel _sceneModel;

    
        void Start()
        {
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
            _canvasFader.OnFadeCompleted += InitializeLoadingScene;

        }
    
        public void ChangeToNextScene(ChangeToNextSceneEvent sceneEvent)
        {
            ChangeToNextScene();
        }
        
        public void ChangeToSpecificScene(ChangeToSpecificSceneEvent sceneEvent)
        {
            ChangeToScene(sceneEvent.SceneName);
        }
    
        private void ChangeToNextScene()
        {
            _sceneModel.PreviousScene = _sceneChangerService.GetCurrentSceneName();
            _sceneModel.NextScene = _sceneChangerService.GetNextSceneFromCurrent();
            StartTransition();
        }

        private void StartTransition()
        {
            _canvasFader.ActivateFader();
            operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.LoadingScene, LoadSceneMode.Single);
            operationLoadingScene.allowSceneActivation = false;
            operationLoadingScene.completed += LoadingSceneLoaded;
        }

        private void ChangeToScene(string scene)
        {
            _sceneModel.PreviousScene = _sceneChangerService.GetCurrentSceneName();
            _sceneModel.NextScene = _sceneChangerService.GetSceneDataByName(scene);
            StartTransition();
        }
    
        private void InitializeLoadingScene()
        {
            _canvasFader.OnFadeCompleted -= InitializeLoadingScene;

            operationLoadingScene.allowSceneActivation = true;
        }
    
        private void OnDestroy()
        {
            _canvasFader.OnFadeCompleted -= InitializeLoadingScene;
        }
    
        private void LoadingSceneLoaded(AsyncOperation obj)
        {
            operationLoadingScene.completed -= LoadingSceneLoaded;
            Debug.Log("Completed LoadingScene");
        }
    }
}