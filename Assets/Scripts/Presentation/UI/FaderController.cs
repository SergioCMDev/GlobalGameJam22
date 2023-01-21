using App.Events;
using App.Models;
using App.SceneManagement;
using Presentation.LoadingScene;
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
            operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.NextScene, LoadSceneMode.Additive);

            // operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.LoadingScene, LoadSceneMode.Single);
            operationLoadingScene.allowSceneActivation = false;
            operationLoadingScene.completed += NextSceneLoaded;
        }
        
        private void NextSceneLoaded(AsyncOperation obj)
        {
            operationLoadingScene.completed -= NextSceneLoaded;
            Debug.Log("Completed LoadingScene");
        }

        private void ChangeToScene(string scene)
        {
            _sceneModel.PreviousScene = _sceneChangerService.GetCurrentSceneName();
            _sceneModel.NextScene = _sceneChangerService.GetSceneDataByName(scene);
            StartTransition();
        }
    
        private void InitializeNextScene()
        {
            _canvasFader.OnFadeCompleted -= InitializeNextScene;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneModel.NextScene));
            SceneManager.UnloadSceneAsync(_sceneModel.PreviousScene);
            operationLoadingScene.allowSceneActivation = true;

            _canvasFader.OnUnfadeCompleted += ChangeToNewScene;
            _canvasFader.DeactivateFader();
        }
    
        private void ChangeToNewScene()
        {
            _canvasFader.OnUnfadeCompleted -= ChangeToNewScene;
            
            // operationNextScene.allowSceneActivation = true;
            // SceneManager.UnloadSceneAsync(_sceneModel.LoadingScene);
            // _canvasFader.DeactivateUI();
        }
        
        private void OnDestroy()
        {
            _canvasFader.OnFadeCompleted -= InitializeNextScene;
        }
    

    }
}