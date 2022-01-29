using Presentation.InputPlayer;
using Presentation.Utils;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Presentation
{
    public class MouseInput : MonoBehaviour
    {
        private ReadInputPlayer _readInputPlayer;
        private bool _pressed;
        private NativeArray<RaycastCommand> _raycastCommands;
        private NativeArray<RaycastHit> _raycastHits;
        private JobHandle _jobHandle;
        [SerializeField] private float _rayLenght, _speed;
        [SerializeField] private string _tagToCheck;
        [SerializeField] private Camera mainCamera;

        private Vector3 _direction;
        private bool _move;
        private Vector3 _worldPoint;

        public Vector3 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        void Update()
        {
            if (UtilsMouse.IsLeftButtonPressedThisFrame())
            {
                _move = true;
                _worldPoint = mainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            }

            if (global::Utils.VectorUtils.VectorIsNearVector(transform.position, _worldPoint, 0.1f))
            {
                _move = false;
            }

            if (_move)
                transform.position = Vector2.MoveTowards(transform.position, _worldPoint, _speed * Time.deltaTime);
        }
    }
}