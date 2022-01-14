// using InputPlayerSystem;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class LevelController : MonoBehaviour
// {
//     [SerializeField] private PlayerInstantiator _playerInstantiator;
//     [SerializeField] private EnemyController _enemyController;
//     [SerializeField] private PlayMusicEvent _playMusicEvent;
//     [SerializeField] private MusicSoundName _musicSoundName;
//     [SerializeField] private CanvasFader _canvasFader;
//     [SerializeField] private CameraFollowPlayer _camera;
//     private SceneChanger _sceneChanger;
//     private ISceneModel _sceneModel;
//     private AsyncOperation operationLoadingScene;
//     private ReadInputPlayer _readInputPlayer;
//
//     void Start()
//     {
//         _sceneModel = ServiceLocator.Instance.GetModel<ISceneModel>();
//         _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();
//         _readInputPlayer = ServiceLocator.Instance.GetService<ReadInputPlayer>();
//         _playerInstantiator.InstantiatePlayer();
//         _playerInstantiator.ResetPlayerPosition();
//         _camera.SetPlayerObject(_playerInstantiator.GetPlayerTransform());
//         _playMusicEvent.soundName = _musicSoundName;
//         _playMusicEvent.Fire();
//
//         _canvasFader.OnFadeCompleted += InitializeLoadingScene;
//     }
//
//     private void OnDestroy()
//     {
//         _canvasFader.OnFadeCompleted -= InitializeLoadingScene;
//     }
//
//     private void InitializeLoadingScene()
//     {
//         operationLoadingScene.allowSceneActivation = true;
//     }
//
//     public void RestartLevel(PlayerHasRestartedLevelEvent playerHasRestartedLevelEvent)
//     {
//         _readInputPlayer.EnableGameplayInput();
//         _playerInstantiator.ResetPlayerPosition();
//         _playerInstantiator.Player.ResetAnimator();
//         _enemyController.ResetSpiderPatrols();
//     }
//
//     public void CompletedLevel(PlayerHasCompletedLevelEvent playerHasCompletedLevelEvent)
//     {
//         _sceneModel.PreviousScene = _sceneChanger.GetCurrentSceneName();
//         _sceneModel.NextScene = _sceneChanger.GetNextSceneFromCurrent();
//         _canvasFader.ActivateFader();
//         operationLoadingScene = SceneManager.LoadSceneAsync(_sceneModel.LoadingScene, LoadSceneMode.Single);
//         operationLoadingScene.allowSceneActivation = false;
//         operationLoadingScene.completed += LoadingSceneLoaded;
//     }
//
//     private void LoadingSceneLoaded(AsyncOperation obj)
//     {
//         operationLoadingScene.completed -= LoadingSceneLoaded;
//         Debug.Log("Completed LoadingScene");
//     }
//
//     //By Event
//     public void ShowLossMenu(PlayerHasLostGameEvent lossGameEventEvent)
//     {
//         _readInputPlayer.DisableGameplayInput();
//         _readInputPlayer.EnableMenusInput();
//     }
// }