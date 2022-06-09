using System;
using App.Events;
using App.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace App.SceneManagement
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private ScenesListModel _scenesListModel;

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

        public void RestartScene(PlayerHasRestartedLevelEvent restartedLevelEvent)
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
    }
}