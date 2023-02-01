using System;
using Presentation.Languages;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Menus
{
    public class InitMenuView : MonoBehaviour
    {
        [SerializeField] private Button buttonStartGame, buttonCredits,buttonOptions, buttonQuitGame;
        // [SerializeField] private TextMeshProUGUI _buttonContinueText, _buttonNewGameText, _buttonOptionsText, _buttonCreditsText, _buttonQuitGameText;

        public event Action OnStartButtonPressed = delegate { };
        public event Action OnCreditsButtonPressed = delegate { };
        public event Action OnShowOptionsMenuButtonPressed = delegate { };
        public event Action OnQuitGameButtonPressed = delegate { };
        

        private void Start()
        {
            // _readInputPlayer.DisableDialogInput();
        }

        public void SetLanguage(ILanguageManager languageManager)
        {
            // _buttonContinueText.SetText(languageManager.GetActualLanguageText().PlayButtonInitSceneKey);
            // _buttonNewGameText.SetText(languageManager.GetActualLanguageText().PlayNewGameInitSceneKey);
            // _buttonOptionsText.SetText(languageManager.GetActualLanguageText().OptionsButtonInitSceneKey);
            // _buttonCreditsText.SetText(languageManager.GetActualLanguageText().CreditsButtonInitSceneKey);
            // _buttonQuitGameText.SetText(languageManager.GetActualLanguageText().QuitGameButtonInitSceneKey);
        }

        private void QuitGame()
        {
            OnQuitGameButtonPressed.Invoke();
        }
        
        private void ShowOptionsMenu()
        {
            OnShowOptionsMenuButtonPressed.Invoke();
        }

        private void ShowCredits()
        {
            OnCreditsButtonPressed.Invoke();
        }

        private void StartGame()
        {
            OnStartButtonPressed.Invoke();
        }
        
        private void OnDestroy()
        {
            DisableInput();
        }

        public void DisableInput()
        {
            buttonCredits.onClick.RemoveListener(ShowCredits);
            buttonOptions.onClick.RemoveListener(ShowOptionsMenu);
            buttonQuitGame.onClick.RemoveListener(QuitGame);
            buttonStartGame.onClick.RemoveListener(StartGame);
        }

        public void EnableInput()
        {
            // _readInputPlayer.OnPlayerPressEnterButtonMenus += HandlePlayerPressEnterButtonMenus;
            buttonStartGame.onClick.AddListener(StartGame);
            buttonCredits.onClick.AddListener(ShowCredits);
            buttonOptions.onClick.AddListener(ShowOptionsMenu);
            buttonQuitGame.onClick.AddListener(QuitGame);
        }
    }
}