using Presentation.Views;
using UnityEngine;

namespace Presentation.Presenters
{
    public class CanvasInitScenePresenter : MonoBehaviour
    {
        [SerializeField] private InitMenuView _initMenuView;
        // [SerializeField] private GameDataStatusLoader _gameDataStatusLoader;
        [SerializeField] private OptionsMenuView _optionsMenuView;
        // private IChangerSceneModel _changerSceneModel;
        // private ILanguageManager _languageManager;

        private void Awake()
        {
            //TODO LOAD GAMEDATA

            // _deleteSavedGameView.OnPlayerDeleteSavedGameData += DeleteLastGameAndStartNewGame;
            // _deleteSavedGameView.OnPlayerCancelDeleteSavedGameData += CloseDeleteLastGameWindow;
            // _gameDataStatusLoader.OnShowContinueButton += ShowContinueButton;
            // _gameDataStatusLoader.OnShowPromptToDeleteLastGame += OpenDeleteLastGameWindow;
            _initMenuView.OnContinueButtonPressed += ContinueGame;
            _initMenuView.OnNewGameButtonPressed += NewGame;
            _initMenuView.OnShowOptionsMenuButtonPressed += ShowOptionsMenu;
            _initMenuView.OnQuitGameButtonPressed += QuitGame;
            _initMenuView.OnShowCreditsButtonPressed += ShowCreditsMenu;

            _optionsMenuView.OnPlayerPressEscapeButton += ShowInitMenu;

            // _changerSceneModel = ServiceLocator.Instance.GetModel<IChangerSceneModel>();
            // _deleteSavedGameView.gameObject.SetActive(false);
            _optionsMenuView.gameObject.SetActive(false);
            _initMenuView.gameObject.SetActive(true);
        }

        private void Start()
        {
            _initMenuView.SetButtons(false);

            // _languageManager = ServiceLocator.Instance.GetService<ILanguageManager>();
            // _initMenuView.SetLanguage(_languageManager);

        }

        private void OnDestroy()
        {
            // _deleteSavedGameView.OnPlayerDeleteSavedGameData -= DeleteLastGameAndStartNewGame;
            // _deleteSavedGameView.OnPlayerCancelDeleteSavedGameData -= CloseDeleteLastGameWindow;
            // _gameDataStatusLoader.OnShowContinueButton -= ShowContinueButton;
            // _gameDataStatusLoader.OnShowPromptToDeleteLastGame -= OpenDeleteLastGameWindow;
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


        private void DeleteLastGameAndStartNewGame()
        {
            // _gameDataStatusLoader.DeleteLastGameAndStartNewGame();
            NewGame();
        }

        private void NewGame()
        {
            //TODO GET REAL SCENE
            // _changerSceneModel.SceneToGo = "LevelStable";
            //
            // _gameDataStatusLoader.StartNewGame();
        }

        private void ContinueGame()
        {
            //TODO GET REAL SCENE

            // _changerSceneModel.SceneToGo = "LevelStable";
            //
            // _gameDataStatusLoader.ContinueGame();
        }

        private void OpenDeleteLastGameWindow()
        {
            _initMenuView.SetCanMoveButtonsStatus(false);
            _initMenuView.DisableInput();

            // _deleteSavedGameView.gameObject.SetActive(true);
        }

        private void CloseDeleteLastGameWindow()
        {
            _initMenuView.SetCanMoveButtonsStatus(true);
            _initMenuView.EnableInput();

            // _deleteSavedGameView.gameObject.SetActive(false);
        }


        private void ShowContinueButton(bool show)
        {
            _initMenuView.SetButtons(show);
        }

        private void ShowOptionsMenu()
        {
            _initMenuView.gameObject.SetActive(false);
            _optionsMenuView.gameObject.SetActive(true);
            _optionsMenuView.ShowOptionsMenu();
        }
    }
}