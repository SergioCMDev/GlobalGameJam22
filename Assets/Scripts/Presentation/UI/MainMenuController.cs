using System;
using App.Events;
using App.SceneManagement;
using Presentation.UI.Menus;
using Services.Popups;
using Services.ScenesChanger;
using UnityEngine;
using Utils;

namespace Presentation.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private PlayMusicEvent playMusicEvent;
        [SerializeField] private MusicSoundName musicSoundName;
        [SerializeField] private CanvasInitScenePresenter canvasInitScenePresenter;
        private SceneChangerService _sceneChangerService;
        [SerializeField] private SceneFaderController sceneFaderController;


        void Start()
        {
            _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();
            canvasInitScenePresenter.OnStartNewGame += StartNewGame;
            canvasInitScenePresenter.OnGoToSelectedScene += GoToSelectedScene;
            playMusicEvent.soundName = musicSoundName;
            playMusicEvent.Fire();
        }

        private void GoToSelectedScene(string sceneName)
        {
            sceneFaderController.GoToSelectedScene(sceneName);
        }

        private void StartNewGame()
        {
            var sceneName = _sceneChangerService.GetFirstSceneName();
            sceneFaderController.GoToSelectedScene(sceneName);
        }
    }
}