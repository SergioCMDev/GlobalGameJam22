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

        void Start()
        {
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
            _canvasInitScenePresenter.OnStartNewGame += StartNewGame;
            _canvasInitScenePresenter.OnGoToSelectedScene += GoToSelectedScene;
            _playMusicEvent.soundName = _musicSoundName;
            _playMusicEvent.Fire();

            _canvasFader.OnFadeCompleted += InitializeLoadingScene;
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
            _canvasFader.ActivateFader();
            operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.LoadingScene, LoadSceneMode.Single);
            operationLoadingScene.allowSceneActivation = false;
            operationLoadingScene.completed += SceneLoaded;        
        }
        
        public void CompletedLevel()
        {
            _sceneModel.PreviousScene = _sceneChangerService.GetCurrentSceneName();
            _sceneModel.NextScene = _sceneChangerService.GetNextSceneFromCurrent();
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