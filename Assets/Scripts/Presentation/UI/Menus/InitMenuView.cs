using System;
using Presentation.InputPlayer;
using Presentation.Languages;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Presentation.UI.Menus
{
    public class InitMenuView : MonoBehaviour
    {
        [SerializeField] private Button _buttonContinue, _buttonNewGame, _buttonOptions, _buttonCredits, _buttonQuitGame;
        // [SerializeField] private TextMeshProUGUI _buttonContinueText, _buttonNewGameText, _buttonOptionsText, _buttonCreditsText, _buttonQuitGameText;
        private ReadInputPlayer _readInputPlayer;

        public event Action OnContinueButtonPressed = delegate { };
        public event Action OnNewGameButtonPressed = delegate { };
        public event Action OnShowOptionsMenuButtonPressed = delegate { };

        public event Action OnShowCreditsButtonPressed = delegate { };
        public event Action OnQuitGameButtonPressed = delegate { };

        

        private void Start()
        {
            _readInputPlayer = ServiceLocator.Instance.GetService<ReadInputPlayer>();
            
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

        private void ShowCreditsMenu()
        {
            OnShowCreditsButtonPressed.Invoke();
        }

        private void ShowOptionsMenu()
        {
            OnShowOptionsMenuButtonPressed.Invoke();
        }

        private void NewGame()
        {
            OnNewGameButtonPressed.Invoke();
        }

        private void ContinueGame()
        {
            OnContinueButtonPressed.Invoke();
        }
        
        private void OnDestroy()
        {
            DisableInput();
        }

   
        public void DisableInput()
        {
            _buttonContinue.onClick.RemoveListener(ContinueGame);
            _buttonNewGame.onClick.RemoveListener(NewGame);
            _buttonOptions.onClick.RemoveListener(ShowOptionsMenu);
            // _buttonCredits.onClick.RemoveListener(ShowCreditsMenu);
            // _buttonQuitGame.onClick.RemoveListener(QuitGame);
        }

        public void EnableInput()
        {
            // _readInputPlayer.OnPlayerPressEnterButtonMenus += HandlePlayerPressEnterButtonMenus;
            _buttonContinue.onClick.AddListener(ContinueGame);
            _buttonNewGame.onClick.AddListener(NewGame);
            _buttonOptions.onClick.AddListener(ShowOptionsMenu);
            // _buttonCredits.onClick.AddListener(ShowCreditsMenu);
            // _buttonQuitGame.onClick.AddListener(QuitGame);
        }

        public void HideContinueButton()
        {
            _buttonContinue.gameObject.SetActive(false);
        }
    }
}