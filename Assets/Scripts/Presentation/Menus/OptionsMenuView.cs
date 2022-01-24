using System;
using System.Collections.Generic;
using Presentation.Input;
using UnityEngine;
using Utils;

namespace Presentation.Menus
{
    public class OptionsMenuView : MonoBehaviour
    {
        private ReadInputPlayer _readInputPlayer;
        [SerializeField] private TabComponent _tabControls;
        [SerializeField] private TabComponent _tabVideo;
        [SerializeField] private TabComponent _tabAudio;
        [SerializeField] private TabComponent _tabMisc;
        private List<TabComponent> _tabButtons = new List<TabComponent>();

        // [SerializeField] private TabsOptionsInitMenuView _tabsOptionsInitMenuView;
        public event Action OnPlayerPressEscapeButton = delegate { };

        private void Awake()
        {
            _tabButtons.Add(_tabControls);
            _tabButtons.Add(_tabVideo);
            _tabButtons.Add(_tabAudio);
            _tabButtons.Add(_tabMisc);

            // _tabsOptionsInitMenuView.OnPlayerPressButtonControls += ShowControlsPanel;
            // _tabsOptionsInitMenuView.OnPlayerPressButtonVideo += ShowVideoPanel;
            // _tabsOptionsInitMenuView.OnPlayerPressButtonAudio += ShowAudioPanel;
            // _tabsOptionsInitMenuView.OnPlayerPressButtonMisc += ShowMiscPanel;

            foreach (var tab in _tabButtons)
            {
                tab.ButtonTabView.SetText(tab.TabName);
                tab.PanelTabView.Init();
            }

            HideAllPanels();
        }

        void Start()
        {
            _readInputPlayer = ServiceLocator.Instance.GetService<ReadInputPlayer>();
            _readInputPlayer.OnEscapeButtonPressed += GoToInitScene;
        }

        private void HideAllPanels()
        {
            foreach (var tab in _tabButtons)
            {
                tab.PanelTabView.gameObject.SetActive(false);
            }
        }

        private void ShowMiscPanel()
        {
            HideAllPanels();

            _tabMisc.PanelTabView.gameObject.SetActive(true);
            _tabMisc.PanelTabView.ShowPanel();
        }

        private void ShowAudioPanel()
        {
            HideAllPanels();

            _tabAudio.PanelTabView.gameObject.SetActive(true);
            _tabAudio.PanelTabView.ShowPanel();
        }

        private void ShowVideoPanel()
        {
            HideAllPanels();

            _tabVideo.PanelTabView.gameObject.SetActive(true);
            _tabVideo.PanelTabView.ShowPanel();
        }

        private void ShowControlsPanel()
        {
            HideAllPanels();

            _tabControls.PanelTabView.gameObject.SetActive(true);
            _tabControls.PanelTabView.ShowPanel();
        }


        private void GoToInitScene()
        {
            OnPlayerPressEscapeButton.Invoke();
        }

        public void ShowOptionsMenu()
        {
            // _tabsOptionsInitMenuView.ShowControls();
        }
    }
}