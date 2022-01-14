using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputPlayerSystem
{
    public class InputEmitter : MonoBehaviour
    {
        [SerializeField]
        private GameplayInput _playerInputActions;
        public event Action OnMouseLeft = delegate {};

        void Awake()
        {
            _playerInputActions = new GameplayInput();

            _playerInputActions.Mouse.MouseLeft.performed += ctx => MouseLeftPressed();
        }

        private void OnEnable()
        {
            _playerInputActions?.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions?.Disable();
        }


        private void MouseLeftPressed()
        {
            OnMouseLeft.Invoke();
        }

    }

}