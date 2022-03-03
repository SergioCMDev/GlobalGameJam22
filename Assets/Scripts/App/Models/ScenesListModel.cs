using System;
using System.Collections.Generic;
using System.Linq;
using App.SceneManagement;
using UnityEngine;

namespace App.Models
{
    public class ScenesListModel : MonoBehaviour
    {
        [SerializeField] private List<SceneInfo> _levelsSceneInfos;
        [SerializeField] private string _creditsSceneName, _firstLevelSceneName, _tutorialSceneName, _mainMenuSceneName;
    
        public SceneInfo GetSceneById(int id)
        {
            var sceneInfo = _levelsSceneInfos.SingleOrDefault(x => x.SceneId == id);
            if (string.IsNullOrEmpty(sceneInfo.SceneName))
            {
                throw new Exception($"Scene Info Of {sceneInfo.SceneName} is empty");
            }

            return sceneInfo;
        }

        public SceneInfo GetSceneByName(string sceneName)
        {
            var sceneInfo = _levelsSceneInfos.SingleOrDefault(x => x.SceneName == sceneName);
            if (sceneInfo.SceneId < 0)
            {
                throw new Exception($"Scene Info Of {sceneName} is < 0");
            }

            return sceneInfo;
        }

        public SceneInfo GetNextScene(string currentSceneName)
        {
            int idCurrentScene = GetSceneByName(currentSceneName).SceneId;
            if (idCurrentScene < 0)
            {
                throw new Exception($"Id of Scene is < 0");
            }

            if (idCurrentScene + 1 <= _levelsSceneInfos.Count)
            {
            
                idCurrentScene++;
            }
            else
            {
                return new SceneInfo {SceneName = _creditsSceneName};
            }

            return GetSceneById(idCurrentScene);
        }

        public int GetSceneCount()
        {
            return _levelsSceneInfos.Count;
        }

        public string GetCreditsSceneName()
        {
            return _creditsSceneName;
        }

        public string GetFirstLevelSceneName()
        {
            return _firstLevelSceneName;
        }

        public string GetTutorialSceneName()
        {
            return _tutorialSceneName;
        }

        public string GetMainMenuSceneName()
        {
            return _mainMenuSceneName;
        }
    }
}