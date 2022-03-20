using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI.Menus
{
    public abstract class ViewWithVerticalButtonsBase : MonoBehaviour
    {
        protected List<Button> interactableButtons = new List<Button>();
        protected List<ButtonView> InteractableButtonsViews = new List<ButtonView>();
        private int _selectedButtonVertical = 0;
        protected bool canMoveButtons = true;


        protected void HandlePlayerPressEnterButtonMenus()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            interactableButtons[_selectedButtonVertical].onClick.Invoke();
        }

        protected void PlayerPressYAxisButtons(float AxisMovement)
        {
            if (!canMoveButtons) return;
            if (AxisMovement < 0)
            {
                _selectedButtonVertical++;
                if (_selectedButtonVertical > interactableButtons.Count - 1)
                {
                    _selectedButtonVertical = 0;
                }
            }
            else
            {
                _selectedButtonVertical--;
                if (_selectedButtonVertical < 0)
                {
                    _selectedButtonVertical = interactableButtons.Count - 1;
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
            _selectedButtonVertical = newIdSelectedButton;
            InitStatusButtonSelected();

        }

        protected void InitStatusButtonSelected()
        {
            DisableSparkleOnRestOfButtons();
            InteractableButtonsViews[_selectedButtonVertical].StartSparkle();
        }

        private void DisableSparkleOnRestOfButtons()
        {
            foreach (var buttonView in InteractableButtonsViews)
            {
                buttonView.EndSparkle();
            }
        }
    }
}