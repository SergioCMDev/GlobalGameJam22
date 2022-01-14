using System;
using InputPlayerSystem;
using UnityEngine;

namespace Presentation.Input
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