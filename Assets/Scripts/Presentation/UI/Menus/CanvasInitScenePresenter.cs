using System;
using App.SceneManagement;
using App.Services;
using Presentation.InputPlayer;
using Presentation.Languages;
using Presentation.MusicEmitter;
using UnityEngine;
using Utils;

namespace Presentation.UI.Menus
{
    public class CanvasInitScenePresenter : MonoBehaviour
    {
        [SerializeField] private InitMenuView _initMenuView;

        [SerializeField] private BackgroundSoundEmitter _backgroundSoundEmitter;

        [SerializeField] private OptionsMenuView _optionsMenuView;
        [SerializeField] private LevelSelectorView _levelSelectorView;

        private GameDataService _gameDataService;
        private ReadInputPlayer _readInputPlayer;

        private SceneChanger _sceneChanger;
        private ILanguageManager _languageManager;
        public event Action OnStartNewGame;
        public event Action<string> OnGoToSelectedScene;

        private void Awake()
        {
            //TODO LOAD GAMEDATA

            _initMenuView.OnContinueButtonPressed += ContinueGame;
            _initMenuView.OnNewGameButtonPressed += NewGame;
            _initMenuView.OnShowOptionsMenuButtonPressed += ShowOptionsMenu;
            _initMenuView.OnQuitGameButtonPressed += QuitGame;
            _initMenuView.OnShowCreditsButtonPressed += ShowCreditsMenu;
            _optionsMenuView.OnPlayerPressEscapeButton += ShowInitMenu;

            _levelSelectorView.OnStartLevelSelected += StartLevelSelected;
            _gameDataService = ServiceLocator.Instance.GetService<GameDataService>();
            _sceneChanger = ServiceLocator.Instance.GetService<SceneChanger>();

            // _deleteSavedGameView.gameObject.SetActive(false);
            _optionsMenuView.gameObject.SetActive(false);
            _levelSelectorView.gameObject.SetActive(false);
            _initMenuView.gameObject.SetActive(true);
            if (!_gameDataService.HasStartedGame())
            {
                _initMenuView.HideContinueButton();
            }
        }

        private void StartLevelSelected(string obj)
        {
            _backgroundSoundEmitter.StopMusic();
            _levelSelectorView.gameObject.SetActive(false);

           var sceneName = _sceneChanger.GetSceneDataByName(obj);
           OnGoToSelectedScene?.Invoke(sceneName);
        }

        private void Start()
        {
            _readInputPlayer = ServiceLocator.Instance.GetService<ReadInputPlayer>();

            _readInputPlayer.DisableGameplayInput();
            _readInputPlayer.EnableMenusInput();
            // _languageManager = ServiceLocator.Instance.GetService<ILanguageManager>();
            // _initMenuView.SetLanguage(_languageManager);
        }


        private void OnDestroy()
        {
            _initMenuView.OnContinueButtonPressed -= ContinueGame;
            _initMenuView.OnNewGameButtonPressed -= NewGame;
            _initMenuView.OnShowOptionsMenuButtonPressed -= ShowOptionsMenu;
            _initMenuView.OnQuitGameButtonPressed -= QuitGame;
            _initMenuView.OnShowCreditsButtonPressed -= ShowCreditsMenu;
            _optionsMenuView.OnPlayerPressEscapeButton -= ShowInitMenu;
        }

        private void ShowCreditsMenu()
        {
            Debug.Log("SHOT CREDITS");
        }

        private void QuitGame()
        {
            // ServiceLocator.Instance.GetService<IGameService>().QuitGame();
        }

        private void ShowInitMenu()
        {
            _optionsMenuView.gameObject.SetActive(false);
            _initMenuView.gameObject.SetActive(true);
        }


        private void NewGame()
        {
            if (OnStartNewGame == null) return;

            _backgroundSoundEmitter.StopMusic();
            OnStartNewGame();
        }

        private void ContinueGame()
        {
            // _backgroundSoundEmitter.StopMusic();

            _initMenuView.DisableInput();
            _initMenuView.gameObject.SetActive(false);
            _levelSelectorView.gameObject.SetActive(true);

          var lastCompletedLevel =  _gameDataService.GetIdOfLastLevelPlayed();
          _levelSelectorView.Init(lastCompletedLevel);
          //TODO GET REAL SCENE

          // _changerSceneModel.SceneToGo = "LevelStable";
          //
          // _gameDataStatusLoader.ContinueGame();
        }
        

        private void ShowOptionsMenu()
        {
            _initMenuView.gameObject.SetActive(false);
            _optionsMenuView.gameObject.SetActive(true);
            _optionsMenuView.ShowOptionsMenu();
        }
    }
}