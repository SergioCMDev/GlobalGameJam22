using App.Events;
using App.Models;
using App.SceneManagement;
using Presentation.LoadingScene;
using Presentation.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private PlayMusicEvent _playMusicEvent;
        [SerializeField] private MusicSoundName _musicSoundName;
        [SerializeField] private CanvasFader _canvasFader;
        [SerializeField] private CanvasInitScenePresenter _canvasInitScenePresenter;
        private SceneChanger _sceneChanger;
        private ISceneModel _sceneModel;
        private AsyncOperation operationLoadingScene;

        void Start()
        {
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
            _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
            _canvasInitScenePresenter.OnStartNewGame += StartNewGame;
            _playMusicEvent.soundName = _musicSoundName;
            _playMusicEvent.Fire();

            _canvasFader.OnFadeCompleted += InitializeLoadingScene;
        }

        private void StartNewGame()
        {
            _sceneModel.PreviousScene = _sceneChanger.GetCurrentSceneName();
            _sceneModel.NextScene = _sceneChanger.GetSceneDataByName("Level1");
            _canvasFader.ActivateFader();
            operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.LoadingScene, LoadSceneMode.Single);
            operationLoadingScene.allowSceneActivation = false;
            operationLoadingScene.completed += SceneLoaded;        
        }

        private void GoToSelectedScene(string levelName)
        {
            _sceneModel.PreviousScene = _sceneChanger.GetCurrentSceneName();
            _sceneModel.NextScene = _sceneChanger.GetSceneDataByName(levelName);
            _canvasFader.ActivateFader();
            operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.LoadingScene, LoadSceneMode.Single);
            operationLoadingScene.allowSceneActivation = false;
            operationLoadingScene.completed += SceneLoaded;        
        }

        
        
        public void CompletedLevel()
        {
            _sceneModel.PreviousScene = _sceneChanger.GetCurrentSceneName();
            _sceneModel.NextScene = _sceneChanger.GetNextSceneFromCurrent();
            _canvasFader.ActivateFader();
            operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.LoadingScene, LoadSceneMode.Single);
            operationLoadingScene.allowSceneActivation = false;
            operationLoadingScene.completed += SceneLoaded;
        }
        
        private void SceneLoaded(AsyncOperation obj)
        {
            operationLoadingScene.completed -= SceneLoaded;
            Debug.Log("Completed LoadingScene");
        }

        private void InitializeLoadingScene()
        {
            operationLoadingScene.allowSceneActivation = true;
        }
    }
}