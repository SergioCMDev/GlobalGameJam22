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
        private SceneChangerService _sceneChangerService;
        [SerializeField] private ChangeToSpecificSceneEvent changeToSpecificSceneEvent;

        void Start()
        {

            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            var operationLoadingScene = SceneManager.LoadSceneAsync(_sceneChangerService.GetFaderSceneName(), LoadSceneMode.Additive);
            canvasInitScenePresenter.OnStartNewGame += StartNewGame;
            canvasInitScenePresenter.OnGoToSelectedScene += GoToSelectedScene;
            playMusicEvent.soundName = musicSoundName;
            playMusicEvent.Fire();
        }

        private void GoToSelectedScene(string sceneName)
        {
            changeToSpecificSceneEvent.SceneName = sceneName;
            changeToSpecificSceneEvent.Fire();
        }

        private void StartNewGame()
        {
            var sceneName = _sceneChangerService.GetFirstSceneName();
            changeToSpecificSceneEvent.SceneName = sceneName;
            changeToSpecificSceneEvent.Fire();
        }
    }
}