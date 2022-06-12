using System;
using Presentation.Interfaces;
using Presentation.UI.Menus;
using TMPro;
using UnityEngine;

namespace Presentation
{
    public class NewRoundPopup : MonoBehaviour, ICloseablePopup
    {
        [SerializeField] private TextMeshProUGUI incomingAttackText;

        public Action<GameObject> HasToClosePopup { get; set; }
        public Action PopupHasBeenClosed { get; set; }

        public void Init(float timeToShow)
        {
            Timer timer = new Timer();
            timer.Init(timeToShow);
            StartCoroutine(timer.CountTime());
            timer.OnTimerEnds += ClosePopup;
        }

        private void ClosePopup()
        {
            HasToClosePopup.Invoke(gameObject);
        }
    }
}