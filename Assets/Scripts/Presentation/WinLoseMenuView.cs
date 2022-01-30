using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation
{
    public class WinLoseMenuView : MonoBehaviour
    {
        [SerializeField] private Image _winImage, _loseImage;

        private void Start()
        {
            _winImage.gameObject.SetActive(false);
            _loseImage.gameObject.SetActive(false);
        }

        public void ShowLoseImage()
        {
            _loseImage.gameObject.SetActive(true);
        }

        public void ShowWinImage()
        {
            _winImage.gameObject.SetActive(true);
        }
    }
}