using System;
using System.Collections.Generic;
using System.Linq;
using App.SceneManagement;

namespace Services.ScenesChanger.Models
{
    public class ScenesListModel 
    {
        private List<SceneInfo> _levelsSceneInfos;
        private List<ConstantScenes> _constantScenesList;

        public void Init(List<SceneInfo> levelsSceneInfos, List<ConstantScenes> constantScenesList)
        {
            _levelsSceneInfos = levelsSceneInfos;
            _constantScenesList = constantScenesList;
        }


        private string GetSceneNameByConstantSceneName(ConstantSceneName constantSceneName)
        {
            foreach (var VARIABLE in _constantScenesList)
            {
                if (VARIABLE.constantSceneName == constantSceneName)
                {
                    return VARIABLE.sceneName;
                }
            }

            return null;
        }
        
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
                return new SceneInfo {SceneName = GetCreditsSceneName()};
            }

            return GetSceneById(idCurrentScene);
        }

        public int GetSceneCount()
        {
            return _levelsSceneInfos.Count;
        }

        public string GetCreditsSceneName()
        {
            return GetSceneNameByConstantSceneName(ConstantSceneName.CreditSceneName);
        }

        public string GetFirstLevelSceneName()
        {
            return GetSceneNameByConstantSceneName(ConstantSceneName.FirstLevelSceneName);
        }

        public string GetTutorialSceneName()
        {
            return GetSceneNameByConstantSceneName(ConstantSceneName.TutorialSceneName);
        }

        public string GetMainMenuSceneName()
        {
            return GetSceneNameByConstantSceneName(ConstantSceneName.MainMenuSceneName);
        }
    }
}