using Services.ScenesChanger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class SplashController : MonoBehaviour
{
    private AsyncOperation operationLoadingScene2;
    private AsyncOperation operationLoadingScene;
    private bool sceneLoaded;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        var _sceneChangerService = ServiceLocator.Instance.GetService<SceneChangerService>();

        operationLoadingScene = SceneManager.LoadSceneAsync(_sceneChangerService.GetMainMenuSceneName(), LoadSceneMode.Single);
        operationLoadingScene.allowSceneActivation = true;
        
       operationLoadingScene2 = SceneManager.LoadSceneAsync(_sceneChangerService.GetFaderSceneName(), LoadSceneMode.Additive);
       operationLoadingScene2.allowSceneActivation = true;
    }
    
    
    public void Update()
    {
        if (sceneLoaded || operationLoadingScene == null) return;
        var progress1 = Mathf.Clamp01(operationLoadingScene.progress / 0.9f) * 100f;
        var progress2 = Mathf.Clamp01(operationLoadingScene2.progress / 0.9f) * 100f;
        Debug.Log($"Progress1 {progress1} Progress2 {progress2}");
        if (progress1 == 100 && progress2 == 100)
        {
            SceneLoadedProgressDone();
        }

        Debug.Log("F333");
    }

    private void SceneLoadedProgressDone()
    {
        sceneLoaded = true;
        operationLoadingScene2.allowSceneActivation = true;
    }
}
