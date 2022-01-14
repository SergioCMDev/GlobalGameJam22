using System;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class SceneUtils
    {
        public static String GetCurrentScene()
        {
            return SceneManager.GetActiveScene().name;
        }
    
        public static int GetCurrentSceneId()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
    }
}