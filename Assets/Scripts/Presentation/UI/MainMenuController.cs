using App.Events;
using Presentation.UI.Menus;
using Services.ScenesChanger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private PlayMusicEvent playMusicEvent;
        [SerializeField] private MusicSoundName musicSoundName;
        [SerializeField] private CanvasInitScenePresenter canvasInitScenePresenter;
        [SerializeField] private ChangeToSpecificSceneEvent changeToSpecificSceneEvent;
        private SceneChangerService _sceneChangerService;
        private IGameStatusModel _gameStatusModel;

        void Start()
        {
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            _gameStatusModel = ServiceLocator.Instance.GetModel<IGameStatusModel>();
            if (!Utilities.SceneIsLoaded(_sceneChangerService.GetFaderSceneName()))
            {
                SceneManager.LoadSceneAsync(_sceneChangerService.GetFaderSceneName(), LoadSceneMode.Additive);
            }
            
            canvasInitScenePresenter.OnStartNewGame += StartNewGame;
            canvasInitScenePresenter.OnGoToSelectedScene += GoToSelectedScene;
            // playMusicEvent.soundName = musicSoundName;
            // playMusicEvent.Fire();
        }

        private void GoToSelectedScene(string sceneName)
        {
            changeToSpecificSceneEvent.SceneName = sceneName;
            changeToSpecificSceneEvent.Fire();
            _gameStatusModel.GameStatus = GameStatus.STARTING_FROM_MENU;
        }

        private void StartNewGame()
        {
            _gameStatusModel.GameStatus = GameStatus.STARTING_FROM_MENU;
            var sceneName = _sceneChangerService.GetFirstSceneName();
            changeToSpecificSceneEvent.SceneName = sceneName;
            changeToSpecificSceneEvent.Fire();
        }
    }
}