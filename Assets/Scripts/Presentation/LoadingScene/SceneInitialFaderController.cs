using System;
using App.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Presentation.LoadingScene
{
    public class SceneInitialFaderController : MonoBehaviour
    {
        // private ISceneModel _sceneModel;
        private float _progress;

        private AsyncOperation operationNextScene;
        [SerializeField] private LoadingSceneFader _canvasFader;
        private bool sceneLoaded;
        public event Action OnRemovingFadeEnds;

        void Start()
        {
            // _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
            _canvasFader.OnUnfadeCompleted += Unfade;
            _canvasFader.DeactivateFader();
        }

        private void Unfade()
        {
            _canvasFader.OnUnfadeCompleted -= Unfade;
            _canvasFader.DeactivateUI();
            OnRemovingFadeEnds?.Invoke();
        }


        // private void ChangeToNewScene()
        // {
        //     _canvasFader.OnUnfadeCompleted -= ChangeToNewScene;
        //     
        //     // SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneModel.NextScene));
        //     // operationNextScene.allowSceneActivation = true;
        //     // SceneManager.UnloadSceneAsync(_sceneModel.LoadingScene);
        //     // _canvasFader.DeactivateUI();
        // }

        private void Update()
        {
            //     if (sceneLoaded || operationNextScene == null && operationNextScene.isDone) return;
            //     _progress = Mathf.Clamp01(operationNextScene.progress / 0.9f) * 100f;
            //     Debug.Log($"Progress {_progress}");
            //     if (_progress == 100)
            //     {
            //         SceneLoaded();
            //     }
            //     _canvasFader.SetText(_progress.ToString());
            // }
        }
    }
}