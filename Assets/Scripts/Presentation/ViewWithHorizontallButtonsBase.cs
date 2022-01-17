using System.Collections.Generic;
using Presentation.Input;
using Presentation.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.Menus
{
    public abstract class ViewWithHorizontalButtonsBase : MonoBehaviour
    {
        protected List<Button> interactableButtons = new List<Button>();
        protected List<ButtonView> interactableButtonsViews = new List<ButtonView>();
        private int _selectedButtonHorizontal = -1;
        protected bool canMoveButtons = true;
        protected ReadInputPlayer readInputPlayer;
    
    
        protected void HandlePlayerPressEnterButton()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            interactableButtons[_selectedButtonHorizontal].onClick.Invoke();
        }


        protected void PlayerPressXAxisButtons(float obj)
        {
            if (!canMoveButtons) return;
            if (obj > 0)
            {
                _selectedButtonHorizontal++;
                if (_selectedButtonHorizontal > interactableButtons.Count - 1)
                {
                    _selectedButtonHorizontal = 0;
                }
            }
            else
            {
                _selectedButtonHorizontal--;
                if (_selectedButtonHorizontal < 0)
                {
                    _selectedButtonHorizontal = interactableButtons.Count - 1;
                }
            }

            InitStatusButtonSelected();
        }


        protected virtual void OnEnable()
        {
            canMoveButtons = true;
        }

        protected virtual void OnDisable()
        {
            canMoveButtons = false;
        }

        protected void SetSelectedButton(int newIdSelectedButton)
        {
            //TODO FIX ÑAPA
            if (newIdSelectedButton == _selectedButtonHorizontal)
            {
                return;
            } 
            _selectedButtonHorizontal = newIdSelectedButton;
            InitStatusButtonSelected();
        }
    

        private void InitStatusButtonSelected()
        {
            DisableSparkleOnRestOfButtons();
            interactableButtonsViews[_selectedButtonHorizontal].StartSparkle();
            HandlePlayerPressEnterButton();
        }

        private void DisableSparkleOnRestOfButtons()
        {
            foreach (var buttonView in interactableButtonsViews)
            {
                buttonView.EndSparkle();
            }
        }
    }
}