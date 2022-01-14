using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputPlayerSystem
{
    public class ReadInputPlayer : MonoBehaviour
    {
        private GameplayInput _playerInputActions;
        private Vector2 _movementInMenuInput;

        public event Action OnPlayerInteractOneTime = delegate { };
        public event Action OnPlayerStartMoving = delegate { };
        public event Action<float> OnPlayerPressVerticalAxisButtons = delegate { };
        public event Action<float> OnPlayerPressHorizontalAxisButtons = delegate { };

        public event Action OnPlayerPressPointBow = delegate { };
        public event Action OnPlayerUnpressPointBow = delegate { };
        public event Action<bool> OnPlayerPressLeftButtonMouse = delegate { };
        public event Action<bool> OnPlayerPressRightButtonMouse = delegate { };

        public event Action OnPlayerPressEnterButton = delegate { };
        public event Action OnPlayerPressSpaceButton = delegate { };
        public event Action OnPlayerUndoPressSpaceButton = delegate { };


        public Vector2 MovementInGameAxis { get; private set; }

        public event Action OnEscapeButtonPressed = delegate { };

        private void OnEnable()
        {
            _playerInputActions?.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions?.Disable();
        }

        private void Awake()
        {
            _playerInputActions = new GameplayInput();


            _playerInputActions.Gameplay.Interact.performed += ctx => InteractOneTime(); //PRESS ONE TIME
            // _playerInputActions.Gameplay.Interact.canceled += ctx => InteractReleased(); //PRESS AND RELEASE

            _playerInputActions.Gameplay.Movement.performed += PlayerMove;
            _playerInputActions.Gameplay.Movement.canceled += PlayerStopMoving;

            // _playerInputActions.Gameplay.AttackRight.performed += ctx => AttackRight();
            //TODO Check cuando dejamos de pulsar
            _playerInputActions.Gameplay.AttackRight.performed += ctx => UndoAttackRight();

            _playerInputActions.Gameplay.ShotBow.performed += ctx => AttackLeft();
            _playerInputActions.Gameplay.ShotBow.canceled += ctx => UndoAttackLeft();
            _playerInputActions.Gameplay.PauseGame.performed += ctx => EscapeButtonPressed();
            _playerInputActions.Gameplay.Jump.performed += ctx => SpaceButtonPressed();
            _playerInputActions.Gameplay.Jump.canceled += ctx => UndoSpaceButtonPressed();

            _playerInputActions.Gameplay.PauseGame.performed += ctx => EscapeButtonPressed();
            _playerInputActions.Gameplay.PauseGame.performed += ctx => EscapeButtonPressed();

            _playerInputActions.Gameplay.PointBow.performed += ctx => PointBow();
            _playerInputActions.Gameplay.PointBow.canceled += ctx => StopPointingBow();

            _playerInputActions.Menus.Escape.performed += ctx => EscapeButtonPressed();
            _playerInputActions.Menus.VerticalMovement.performed += MoveYAxis;
            _playerInputActions.Menus.HorizontalMovement.performed += MoveXAxis;
            _playerInputActions.Menus.Enter.performed += EnterPressedOnMenu;
        }

        private void StopPointingBow()
        {
            OnPlayerUnpressPointBow.Invoke();
        }

        private void PointBow()
        {
            OnPlayerPressPointBow.Invoke();
        }

        private void UndoSpaceButtonPressed()
        {
            OnPlayerUndoPressSpaceButton.Invoke();
        }

        private void SpaceButtonPressed()
        {
            OnPlayerPressSpaceButton.Invoke();
        }


        private void OnDestroy()
        {
            if (_playerInputActions != null)
            {
                _playerInputActions.Gameplay.Movement.performed -= PlayerMove;
                _playerInputActions.Gameplay.Movement.canceled -= PlayerStopMoving;

                _playerInputActions.Menus.VerticalMovement.performed -= MoveYAxis;
                _playerInputActions.Menus.HorizontalMovement.performed -= MoveXAxis;
                _playerInputActions.Menus.Enter.performed -= EnterPressedOnMenu;
            }
        }


        private void EnterPressedOnMenu(InputAction.CallbackContext obj)
        {
            OnPlayerPressEnterButton.Invoke();
        }


        private void MoveYAxis(InputAction.CallbackContext obj)
        {
            OnPlayerPressVerticalAxisButtons.Invoke(obj.ReadValue<float>());
        }

        private void MoveXAxis(InputAction.CallbackContext obj)
        {
            OnPlayerPressHorizontalAxisButtons.Invoke(obj.ReadValue<float>());
        }


        private void EscapeButtonPressed()
        {
            OnEscapeButtonPressed.Invoke();
        }


        private void PlayerStopMoving(InputAction.CallbackContext obj)
        {
            this.MovementInGameAxis = Vector2.zero;
        }

        private void PlayerMove(InputAction.CallbackContext obj)
        {
            if (obj.ReadValue<Vector2>().magnitude > 0.01f)
            {
                this.MovementInGameAxis = obj.ReadValue<Vector2>();
                OnPlayerStartMoving.Invoke();
            }
        }

        private void UndoAttackLeft()
        {
            // OnPlayerPressLeftButtonMouse.Invoke(false);
        }

        private void UndoAttackRight()
        {
            OnPlayerPressRightButtonMouse.Invoke(false);
        }

        private void AttackLeft()
        {
            OnPlayerPressLeftButtonMouse.Invoke(true);
        }

        private void AttackRight()
        {
            OnPlayerPressRightButtonMouse.Invoke(true);
        }

        private void InteractOneTime()
        {
            Debug.Log("PRESS ONE TIME");

            OnPlayerInteractOneTime.Invoke();
        }

        private void InteractReleased()
        {
            Debug.Log("RELEASE");
        }

        public void DisableGameplayInput()
        {
            _playerInputActions.Gameplay.Disable();
        }

        public void EnableGameplayInput()
        {
            _playerInputActions.Gameplay.Enable();
        }

        public void DisableMenusInput()
        {
            _playerInputActions.Menus.Disable();
        }

        public void EnableMenusInput()
        {
            _playerInputActions.Menus.Enable();
        }
    }
}