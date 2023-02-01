using System;
using System.Collections.Generic;
using System.Linq;
using Presentation.Languages;
using Presentation.Managers;
using Presentation.MusicEmitter;
using Services.GameData;
using Services.ScenesChanger;
using UnityEngine;
using Utils;

namespace Presentation.UI.Menus
{
    public class CanvasInitScenePresenter : MonoBehaviour
    {
        [SerializeField] private InitMenuView initMenuView;
        [SerializeField] private OptionsMenuView optionsMenuView;
        [SerializeField] private List<LevelViewInfo> levelViews;
        // [SerializeField] private BackgroundSoundEmitter backgroundSoundEmitter;
        [SerializeField] private LevelSelectorView levelSelectorView;

        private GameDataService _gameDataService;
        private SceneChangerService _sceneChangerOld;
        private ILanguageManager _languageManager;
        
        public event Action OnStartNewGame;
        public event Action<string> OnGoToSelectedScene;


        private int _selectedLevelId;
        private void Awake()
        {
            //TODO LOAD GAMEDATA

            initMenuView.OnStartButtonPressed += StartGame;
            initMenuView.OnCreditsButtonPressed += Credits;
            initMenuView.OnQuitGameButtonPressed += QuitGame;
            initMenuView.OnShowOptionsMenuButtonPressed += ShowOptions;
            
            levelSelectorView.OnStartLevelSelected += StartSelectedLevel;
            levelSelectorView.OnButtonBackIsPressed += ShowInitMenu;
            levelSelectorView.OnLeftButtonIsPressed += MoveLevelImageToLeft;
            levelSelectorView.OnRightButtonIsPressed += MoveLevelImageToRight;
            
            optionsMenuView.OnBackButtonPressed += ShowInitMenu;
            
            // _deleteSavedGameView.gameObject.SetActive(false);
            levelSelectorView.gameObject.SetActive(false);
            optionsMenuView.gameObject.SetActive(false);
            initMenuView.gameObject.SetActive(true);
            initMenuView.EnableInput();
            SetLevelImage();

        }

        private void ShowOptions()
        {
            initMenuView.DisableInput();
            initMenuView.gameObject.SetActive(false);
            optionsMenuView.Init();
            optionsMenuView.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            initMenuView.OnStartButtonPressed -= StartGame;
            initMenuView.OnCreditsButtonPressed -= Credits;
            initMenuView.OnQuitGameButtonPressed -= QuitGame;
            
            levelSelectorView.OnStartLevelSelected -= StartSelectedLevel;
            levelSelectorView.OnButtonBackIsPressed -= ShowInitMenu;
            levelSelectorView.OnLeftButtonIsPressed -= MoveLevelImageToLeft;
            levelSelectorView.OnRightButtonIsPressed -= MoveLevelImageToRight;
        }

        private void MoveLevelImageToRight()
        {
            _selectedLevelId++;
            SetLevelImage();
        }

        private void SetLevelImage()
        {
            _selectedLevelId = _selectedLevelId.CircularClamp(0, levelViews.Count - 1);
            var levelView = levelViews.Single(x => x.levelId == _selectedLevelId);

            levelSelectorView.SetLevelImage(levelView.levelImage);
        }

        private void MoveLevelImageToLeft()
        {
            _selectedLevelId--;
            SetLevelImage();
        }


        private void StartSelectedLevel()
        {
            // backgroundSoundEmitter.StopMusic();
            levelSelectorView.gameObject.SetActive(false);
            var levelView = levelViews.Single(x => x.levelId == _selectedLevelId);
            
           var sceneName = _sceneChangerOld.GetSceneDataByName(levelView.sceneToLoad);
           OnGoToSelectedScene?.Invoke(sceneName);
        }

        private void Start()
        {
            _gameDataService = ServiceLocator.Instance.GetService<GameDataService>();
            _sceneChangerOld = ServiceLocator.Instance.GetService<SceneChangerService>();
            
            
            if (!_gameDataService.HasStartedGame())
            {
                // initMenuView.HideContinueButton();
            }
            // _languageManager = ServiceLocator.Instance.GetService<ILanguageManager>();
            // _initMenuView.SetLanguage(_languageManager);
        }
        
        private void ShowCreditsMenu()
        {
            initMenuView.DisableInput();

            Debug.Log("SHOT CREDITS");
        }

        private void QuitGame()
        {
        }

        private void ShowInitMenu()
        {
            levelSelectorView.gameObject.SetActive(false);
            optionsMenuView.gameObject.SetActive(false);
            optionsMenuView.Hide();
            initMenuView.gameObject.SetActive(true);
            initMenuView.EnableInput();
        }

        private void Credits()
        {
            
            // backgroundSoundEmitter.StopMusic();
        }

        private void StartGame()
        {
            initMenuView.gameObject.SetActive(false);
            levelSelectorView.gameObject.SetActive(true);
            OnStartNewGame?.Invoke();

          // var lastCompletedLevel =  _gameDataService.GetIdOfLastLevelPlayed();
          // levelSelectorView.Init(lastCompletedLevel);
          //TODO GET REAL SCENE

          // _changerSceneModel.SceneToGo = "LevelStable";
          //
          // _gameDataStatusLoader.ContinueGame();
        }
    }
}