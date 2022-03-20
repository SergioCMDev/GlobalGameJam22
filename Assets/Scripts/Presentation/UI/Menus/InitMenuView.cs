using System;
using Presentation.InputPlayer;
using Presentation.Languages;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Presentation.UI.Menus
{
    public class InitMenuView : ViewWithVerticalButtonsBase
    {
        [SerializeField] private Button _buttonContinue, _buttonNewGame, _buttonOptions, _buttonCredits, _buttonQuitGame;
        // [SerializeField] private TextMeshProUGUI _buttonContinueText, _buttonNewGameText, _buttonOptionsText, _buttonCreditsText, _buttonQuitGameText;
        private ReadInputPlayer _readInputPlayer;

        public event Action OnContinueButtonPressed = delegate { };
        public event Action OnNewGameButtonPressed = delegate { };
        public event Action OnShowOptionsMenuButtonPressed = delegate { };

        public event Action OnShowCreditsButtonPressed = delegate { };
        public event Action OnQuitGameButtonPressed = delegate { };


        private void Awake()
        {
            _buttonContinue.interactable = false;
        }

        private void Start()
        {
            _readInputPlayer = ServiceLocator.Instance.GetService<ReadInputPlayer>();
            _readInputPlayer.OnPlayerPressVerticalAxisButtons += PlayerPressYAxisButtons;
            _readInputPlayer.OnPlayerPressEnterButton += HandlePlayerPressEnterButtonMenus;
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


        public void SetButtons(bool showContinueButton)
        {
            if (showContinueButton)
            {
                interactableButtons.Add(_buttonContinue);
                InteractableButtonsViews.Add(_buttonContinue.GetComponent<ButtonView>());
            }
            else
            {
                _buttonContinue.GetComponent<ButtonView>().SetColorText(Color.gray);
            }

            // _buttonContinue.onClick.AddListener(ContinueGame);
            _buttonNewGame.onClick.AddListener(NewGame);
            // _buttonOptions.onClick.AddListener(ShowOptionsMenu);
            // _buttonCredits.onClick.AddListener(ShowCreditsMenu);
            // _buttonQuitGame.onClick.AddListener(QuitGame);

            interactableButtons.Add(_buttonNewGame);
            // interactableButtons.Add(_buttonOptions);
            // interactableButtons.Add(_buttonCredits);
            // interactableButtons.Add(_buttonQuitGame);

            InteractableButtonsViews.Add(_buttonNewGame.GetComponent<ButtonView>());
            // InteractableButtonsViews.Add(_buttonOptions.GetComponent<ButtonView>());
            // InteractableButtonsViews.Add(_buttonCredits.GetComponent<ButtonView>());
            // InteractableButtonsViews.Add(_buttonQuitGame.GetComponent<ButtonView>());


            SetSelectedButton(0);

            InitStatusButtonSelected();
        }

        private void OnDestroy()
        {
            DisableInput();
        }

        public void SetCanMoveButtonsStatus(bool statusButtons)
        {
            canMoveButtons = statusButtons;
        }

        public void DisableInput()
        {
            _readInputPlayer.OnPlayerPressVerticalAxisButtons -= PlayerPressYAxisButtons;
            // _readInputPlayer.OnPlayerPressEnterButtonMenus -= HandlePlayerPressEnterButtonMenus;
            _buttonContinue.onClick.RemoveListener(ContinueGame);
            _buttonNewGame.onClick.RemoveListener(NewGame);
            _buttonOptions.onClick.RemoveListener(ShowOptionsMenu);
            // _buttonCredits.onClick.RemoveListener(ShowCreditsMenu);
            // _buttonQuitGame.onClick.RemoveListener(QuitGame);
        }

        public void EnableInput()
        {
            _readInputPlayer.OnPlayerPressVerticalAxisButtons += PlayerPressYAxisButtons;
            // _readInputPlayer.OnPlayerPressEnterButtonMenus += HandlePlayerPressEnterButtonMenus;
            _buttonContinue.onClick.AddListener(ContinueGame);
            _buttonNewGame.onClick.AddListener(NewGame);
            _buttonOptions.onClick.AddListener(ShowOptionsMenu);
            // _buttonCredits.onClick.AddListener(ShowCreditsMenu);
            // _buttonQuitGame.onClick.AddListener(QuitGame);
        }
    }
}