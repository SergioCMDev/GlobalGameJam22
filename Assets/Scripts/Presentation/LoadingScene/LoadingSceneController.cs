using Application_.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation.LoadingScene
{
    public class LoadingSceneController : MonoBehaviour
    {
        private ISceneModel _sceneModel;
        private float _progress;

        private AsyncOperation operationNextScene;
        [SerializeField] private LoadingSceneFader _canvasFader;

        void Start()
        {
            _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();

            operationNextScene = SceneManager.LoadSceneAsync(_sceneModel.NextScene, LoadSceneMode.Additive);
            operationNextScene.completed += SceneLoaded;
        }

        private void SceneLoaded(AsyncOperation obj)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneModel.NextScene));
            operationNextScene.allowSceneActivation = true;

            operationNextScene.completed -= SceneLoaded;
            _canvasFader.OnUnfadeCompleted += ChangeToNewScene;
            _canvasFader.DeactivateUI();
            _canvasFader.DeactivateFader();
        }

        private void ChangeToNewScene()
        {
            SceneManager.UnloadSceneAsync(_sceneModel.LoadingScene);
            _canvasFader.OnUnfadeCompleted -= ChangeToNewScene;
        }

        private void Update()
        {
            if (operationNextScene.isDone) return;
            _progress = Mathf.Clamp01(operationNextScene.progress / 0.9f) * 100f;
            Debug.Log($"Progress {_progress}");
            _canvasFader.SetText(_progress.ToString());
        }
    }
}