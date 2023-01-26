using System;
using System.Collections.Generic;
using App.Events;
using App.SceneManagement;
using Services.ScenesChanger.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Services.ScenesChanger
{
    [CreateAssetMenu(fileName = "SceneChangerService",
        menuName = "Loadable/Services/SceneChangerService")]
    public class SceneChangerService : LoadableComponent
    {
        // [SerializeField] private ChangeToSpecificSceneEvent _changeToSpecificSceneEvent;

        [SerializeField] private List<SceneInfo> _levelsSceneInfoList;
        [SerializeField] private List<ConstantScenes> constantScenesList;
        
        private ScenesListModel _scenesListModel;

        public override void Execute()
        {
            Debug.Log($"Execute SceneChangerService");
            _scenesListModel = new ScenesListModel();
            _scenesListModel.Init(_levelsSceneInfoList, constantScenesList);
        }

        private SceneInfo GetCurrentSceneInfoById(int id)
        {
            var currentSceneInfo =
                _scenesListModel.GetSceneById(id);
            return currentSceneInfo;
        }

        private SceneInfo GetCurrentSceneInfoByName(string levelName)
        {
            var currentSceneInfo =
                _scenesListModel.GetSceneByName(levelName);
            return currentSceneInfo;
        }

        private void GoToNextLevel(SceneInfo actualScene)
        {
            SceneInfo nextSceneInfo = _scenesListModel.GetNextScene(actualScene.SceneName);

            Debug.Log($"Cambiamos a nivel {actualScene.SceneName}");

            SceneManager.LoadScene(nextSceneInfo.SceneName);
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneUtils.GetCurrentScene());
        }

        public void GoToFirstLevel()
        {
            SceneManager.LoadScene(_scenesListModel.GetFirstLevelSceneName());
        }

        public void GoToCreditsScene(OpenCreditsSceneEvent openCreditsSceneEvent)
        {
            SceneManager.LoadScene(_scenesListModel.GetCreditsSceneName());
        }


        public void GoToNextScene()
        {
            SceneManager.LoadScene(SceneUtils.GetCurrentSceneId() + 1);
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene(_scenesListModel.GetMainMenuSceneName());
        }

        public String GetCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        public void LoadCurrentLevelFromInitScene(int gameDataLastLevelCompleted)
        {
            if (gameDataLastLevelCompleted + 1 >= _scenesListModel.GetSceneCount())
            {
                SceneManager.LoadScene(_scenesListModel.GetSceneById(_scenesListModel.GetSceneCount() - 1).SceneName);
                return;
            }


            SceneManager.LoadScene(_scenesListModel.GetSceneById(gameDataLastLevelCompleted + 1).SceneName);
        }

        public void GoToLevelScene(int levelId)
        {
            var sceneInfo = GetCurrentSceneInfoById(levelId);
            SceneManager.LoadScene(sceneInfo.SceneName);
        }

        public void GoToLevelScene(string levelName)
        {
            var sceneInfo = GetCurrentSceneInfoByName(levelName);
            SceneManager.LoadScene(sceneInfo.SceneName);
        }

        public string GetNextSceneFromCurrent()
        {
            SceneInfo nextSceneInfo = _scenesListModel.GetNextScene(GetCurrentSceneName());
            return nextSceneInfo.SceneName;
        }

        public string GetSceneDataByName(string levelName)
        {
            if (levelName == GetMainMenuSceneName())
            {
                return GetMainMenuSceneName();
            }

            var sceneInfo = GetCurrentSceneInfoByName(levelName);
            return sceneInfo.SceneName;
        }

        public string GetMainMenuSceneName()
        {
            return _scenesListModel.GetMainMenuSceneName();
        }

        public string GetFirstSceneName()
        {
            return _scenesListModel.GetFirstLevelSceneName();
        }

        public string GetFaderSceneName()
        {
            return _scenesListModel.GetFaderMenuSceneName();
        }
    }

    public enum ConstantSceneName
    {
        CreditSceneName,
        FirstLevelSceneName,
        TutorialSceneName,
        MainMenuSceneName,
        FaderSceneName
    }
    [Serializable]
    public struct ConstantScenes
    {
        public ConstantSceneName constantSceneName;
        public string sceneName;
    }
}