﻿using System;
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

        // [SerializeField] private GameDataService _gameDataService;
        private ReadInputPlayer _readInputPlayer;

        // private SceneChanger _sceneChanger;
        private ILanguageManager _languageManager;
        public event Action OnStartNewGame;

        private void Awake()
        {
            //TODO LOAD GAMEDATA


            _initMenuView.OnContinueButtonPressed += ContinueGame;
            _initMenuView.OnNewGameButtonPressed += NewGame;
            _initMenuView.OnShowOptionsMenuButtonPressed += ShowOptionsMenu;
            _initMenuView.OnQuitGameButtonPressed += QuitGame;
            _initMenuView.OnShowCreditsButtonPressed += ShowCreditsMenu;
            _optionsMenuView.OnPlayerPressEscapeButton += ShowInitMenu;

            // _gameDataService = ServiceLocator.Instance.GetService<GameDataService>();


            // _deleteSavedGameView.gameObject.SetActive(false);
            _optionsMenuView.gameObject.SetActive(false);
            _initMenuView.gameObject.SetActive(true);
        }

        private void Start()
        {
            _readInputPlayer = ServiceLocator.Instance.GetService<ReadInputPlayer>();

            _readInputPlayer.DisableGameplayInput();
            _readInputPlayer.EnableMenusInput();
            // _languageManager = ServiceLocator.Instance.GetService<ILanguageManager>();
            // _initMenuView.SetLanguage(_languageManager);
            ShowContinueButton(false);
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
            _backgroundSoundEmitter.StopMusic();
            //TODO GET REAL SCENE

            // _changerSceneModel.SceneToGo = "LevelStable";
            //
            // _gameDataStatusLoader.ContinueGame();
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