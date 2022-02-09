using Application_.Models;
using Application_.SceneManagement;
using Presentation.LoadingScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class FaderController : MonoBehaviour
{
    
    [SerializeField] private CanvasFader _canvasFader;
    private SceneChanger _sceneChanger;
    private AsyncOperation operationLoadingScene;
    private ISceneModel _sceneModel;

    
    void Start()
    {
        _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
        _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
        _canvasFader.OnFadeCompleted += InitializeLoadingScene;

    }
    public void ChangeToNextScene()
    {
        _sceneModel.PreviousScene = _sceneChanger.GetCurrentSceneName();
        _sceneModel.NextScene = _sceneChanger.GetNextSceneFromCurrent();
        _canvasFader.ActivateFader();
        operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.LoadingScene, LoadSceneMode.Single);
        operationLoadingScene.allowSceneActivation = false;
        operationLoadingScene.completed += LoadingSceneLoaded;
    }
    
    private void InitializeLoadingScene()
    {
        _canvasFader.OnFadeCompleted -= InitializeLoadingScene;

        operationLoadingScene.allowSceneActivation = true;
    }
    
    private void OnDestroy()
    {
        _canvasFader.OnFadeCompleted -= InitializeLoadingScene;
    }
    
    private void LoadingSceneLoaded(AsyncOperation obj)
    {
        operationLoadingScene.completed -= LoadingSceneLoaded;
        Debug.Log("Completed LoadingScene");
    }
}